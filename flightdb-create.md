# FlightDb — Schema mẫu học .NET (rút gọn từ AODB ICD)

> Nguồn ý tưởng: *API AODB ICD ver 1_JSON*  
> Mục tiêu: học EF Core / CRUD — **không** bám sát đủ ~49 field AODB.  
> Rút còn **3 bảng**: Users + Airports + Flights.

## Sơ đồ quan hệ

```
Users
Airports (master IATA)
Flights ──FK──> Airports (Origin, Destination)
```

## Field AODB giữ lại / bỏ

| Giữ (học đủ) | Bỏ (CDM / baggage / ground handling chi tiết) |
|---|---|
| FlightNo, FlightDate, Route → Origin/Dest | ACGT, AEGT, MGHA |
| Status, ArrDep | Bag/Cgo/Mail Pcs & Kgs |
| ACRegNo, ACType, FlightType, NatureOfFlight | EXIT, EXOT, ETTT, TOBT, CTOT, TSAT… |
| Schedule / Estimate / Actual (1 bộ giờ) | ASBT, ARDT, ASAT, TTOT, ETOT |
| Parking, Gate | Belt, Chute, CkiRow, Runway… |
| Booking (số khách) | OPENCHUTE, CLOSECHUTE… |

Giờ lưu `DATETIME2` (dễ dùng EF) thay vì chuỗi `1105` / `1105+` của AODB.

---

## SQL — SQL Server

Chạy trên database `FlightDb` (đúng connection string trong `appsettings.json`).

**Chỉ copy khối SQL bên dưới** (không copy phần markdown).

```sql
USE master;
GO

IF DB_ID(N'FlightDb') IS NULL
    CREATE DATABASE FlightDb;
GO

USE FlightDb;
GO

/* ========== DROP (dev / học lại từ đầu) ========== */
IF OBJECT_ID(N'dbo.Flights', N'U') IS NOT NULL DROP TABLE dbo.Flights;
IF OBJECT_ID(N'dbo.Airports', N'U') IS NOT NULL DROP TABLE dbo.Airports;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
GO

/* ========== 1. Users ========== */
CREATE TABLE dbo.Users (
    Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Users PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    Username    NVARCHAR(50)  NOT NULL,
    Password    NVARCHAR(255) NOT NULL,
    FullName    NVARCHAR(100) NOT NULL,
    Email       NVARCHAR(100) NOT NULL,
    CreatedAt   DATETIME2     NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),

    CONSTRAINT UQ_Users_Username UNIQUE (Username),
    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);
GO

/* ========== 2. Airports — master IATA ========== */
CREATE TABLE dbo.Airports (
    Id       UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Airports PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    IataCode CHAR(3)       NOT NULL,           -- DLI, HAN, SGN
    Name     NVARCHAR(150) NOT NULL,
    City     NVARCHAR(100) NULL,
    Country  NVARCHAR(100) NULL CONSTRAINT DF_Airports_Country DEFAULT N'Vietnam',

    CONSTRAINT UQ_Airports_IataCode UNIQUE (IataCode)
);
GO

/* ========== 3. Flights — bản rút gọn AODB ========== */
CREATE TABLE dbo.Flights (
    Id               UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Flights PRIMARY KEY DEFAULT NEWSEQUENTIALID(),

    -- Định danh chuyến (AODB: FlightNo + FlightDate + Route)
    FlightNo         NVARCHAR(10)  NOT NULL,   -- VN7324
    FlightDate       DATE          NOT NULL,   -- 2025-10-15
    ArrDep           CHAR(1)       NOT NULL,   -- A | D
    Status           NVARCHAR(3)   NOT NULL,   -- OPN, DLY, CNX, CLS, XXX

    OriginAirportId      UNIQUEIDENTIFIER NOT NULL,
    DestinationAirportId UNIQUEIDENTIFIER NOT NULL,

    -- Máy bay / loại chuyến
    AircraftReg      NVARCHAR(20)  NULL,       -- ACRegNo, e.g. VNA324
    AircraftType     NVARCHAR(10)  NOT NULL,   -- ACType ICAO, e.g. A320
    FlightType       NVARCHAR(3)   NOT NULL,   -- PAX | CGO
    NatureOfFlight   NVARCHAR(3)   NOT NULL CONSTRAINT DF_Flights_Nature DEFAULT N'---',
                                                   -- ---, CHT, DIV, FER...

    -- Giờ: Schedule / Estimate / Actual
    -- Arrival ~ SIBT/EIBT/AIBT
    -- Departure ~ SOBT/EOBT/AOBT
    ScheduledTime    DATETIME2     NULL,
    EstimatedTime    DATETIME2     NULL,
    ActualTime       DATETIME2     NULL,

    ParkingStand     NVARCHAR(10)  NULL,       -- APRK / DPRK
    Gate             NVARCHAR(10)  NULL,       -- AGATE / DGATE
    BookingPax       INT           NULL,       -- Booking

    CreatedAt        DATETIME2     NOT NULL CONSTRAINT DF_Flights_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt        DATETIME2     NULL,

    CONSTRAINT CK_Flights_ArrDep CHECK (ArrDep IN ('A', 'D')),
    CONSTRAINT CK_Flights_Status CHECK (Status IN ('OPN', 'DLY', 'CNX', 'CLS', 'XXX')),
    CONSTRAINT CK_Flights_FlightType CHECK (FlightType IN ('PAX', 'CGO')),
    CONSTRAINT CK_Flights_OriginDest CHECK (OriginAirportId <> DestinationAirportId),

    CONSTRAINT FK_Flights_Origin
        FOREIGN KEY (OriginAirportId) REFERENCES dbo.Airports(Id),
    CONSTRAINT FK_Flights_Destination
        FOREIGN KEY (DestinationAirportId) REFERENCES dbo.Airports(Id),

    CONSTRAINT UQ_Flights_Identity UNIQUE (FlightNo, FlightDate, OriginAirportId, DestinationAirportId, ArrDep)
);
GO

CREATE INDEX IX_Flights_FlightDate ON dbo.Flights (FlightDate);
CREATE INDEX IX_Flights_Status ON dbo.Flights (Status);
GO

/* ========== Seed mẫu ========== */
INSERT INTO dbo.Users (Id, Username, Password, FullName, Email) VALUES
(NEWID(), N'admin', N'123456', N'System Admin', N'admin@flight.local'),
(NEWID(), N'bao', N'123456', N'Nguyen Gia Bao', N'bao@flight.local');

DECLARE @HAN UNIQUEIDENTIFIER = NEWID();
DECLARE @SGN UNIQUEIDENTIFIER = NEWID();
DECLARE @DLI UNIQUEIDENTIFIER = NEWID();
DECLARE @DAD UNIQUEIDENTIFIER = NEWID();

INSERT INTO dbo.Airports (Id, IataCode, Name, City) VALUES
(@HAN, 'HAN', N'Noi Bai International Airport', N'Ha Noi'),
(@SGN, 'SGN', N'Tan Son Nhat International Airport', N'Ho Chi Minh City'),
(@DLI, 'DLI', N'Lien Khuong Airport', N'Da Lat'),
(@DAD, 'DAD', N'Da Nang International Airport', N'Da Nang');

-- Bảng do EF tạo thường không có DEFAULT cho Id → bắt buộc ghi NEWID()
INSERT INTO dbo.Flights (
    Id, FlightNo, FlightDate, ArrDep, Status,
    OriginAirportId, DestinationAirportId,
    AircraftReg, AircraftType, FlightType, NatureOfFlight,
    ScheduledTime, EstimatedTime, ActualTime,
    ParkingStand, Gate, BookingPax
) VALUES
-- Departure DLI → HAN (giống ví dụ ICD VN7324)
(NEWID(), 'VN7324', '2025-10-15', 'D', 'OPN',
 @DLI, @HAN,
 'VNA324', 'AT72', 'PAX', '---',
 '2025-10-15T08:35:00', '2025-10-15T08:35:00', NULL,
 '01', '2', 70),

-- Arrival HAN ← SGN
(NEWID(), 'VJ180', '2025-10-15', 'A', 'OPN',
 @SGN, @HAN,
 NULL, 'A320', 'PAX', '---',
 '2025-10-15T10:00:00', '2025-10-15T10:15:00', NULL,
 '60A', '18', 180),

-- Delayed departure HAN → SGN
(NEWID(), 'VN210', '2025-10-15', 'D', 'DLY',
 @HAN, @SGN,
 'VNA662', 'A321', 'PAX', '---',
 '2025-10-15T14:00:00', '2025-10-15T15:30:00', NULL,
 '12', '5', 200);
GO

/* ========== Kiểm tra ========== */
SELECT Username, FullName, Email FROM dbo.Users;

SELECT a.IataCode, a.Name FROM dbo.Airports a ORDER BY a.IataCode;

SELECT
    f.FlightNo,
    f.FlightDate,
    f.ArrDep,
    f.Status,
    o.IataCode AS Origin,
    d.IataCode AS Destination,
    f.AircraftType,
    f.ScheduledTime,
    f.EstimatedTime,
    f.Gate
FROM dbo.Flights f
JOIN dbo.Airports o ON o.Id = f.OriginAirportId
JOIN dbo.Airports d ON d.Id = f.DestinationAirportId
ORDER BY f.FlightDate, f.ScheduledTime;
```

---

## Map nhanh AODB → cột DB

| AODB | Cột `Flights` |
|------|----------------|
| FlightNo | `FlightNo` |
| FlightDate | `FlightDate` |
| Route (HAN-SGN) | `OriginAirportId` + `DestinationAirportId` |
| ArrDep | `ArrDep` |
| Status | `Status` |
| ACRegNo / ACType | `AircraftReg` / `AircraftType` |
| FlightType / NatureOfFlight | `FlightType` / `NatureOfFlight` |
| SIBT/SOBT | `ScheduledTime` |
| EIBT/EOBT | `EstimatedTime` |
| AIBT/AOBT | `ActualTime` |
| APRK/DPRK | `ParkingStand` |
| AGATE/DGATE | `Gate` |
| Booking | `BookingPax` |

---

## Gợi ý bước tiếp (.NET)

1. Chạy script trên SQL Server.  
2. Tạo entity `User`, `Airport`, `Flight` + `DbSet` trong `ApplicationDbContext`.  
3. `dotnet ef migrations add InitialCreate` (hoặc scaffold nếu muốn).  
4. CRUD theo pattern Controller → Service → Repository + DTO.

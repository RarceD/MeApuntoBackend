
CREATE TABLE Scheduler (
    Id       INTEGER NOT NULL
                     PRIMARY KEY AUTOINCREMENT,
    CourtId  INTEGER,
    Time     TEXT,
    ClientId INTEGER
);

CREATE TABLE Configuration (
    Id        INTEGER PRIMARY KEY AUTOINCREMENT,
    ValidHour TEXT,
    CourtId   INTEGER
);


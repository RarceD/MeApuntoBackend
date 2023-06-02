CREATE TABLE urbas (
    id           INTEGER NOT NULL,
    name         TEXT,
    info         TEXT,
    [key]        TEXT,
    advance_book INTEGER,
    PRIMARY KEY (
        id
    )
);

CREATE TABLE clients (
    id       INTEGER,
    urba_id  INTEGER,
    name     TEXT,
    username TEXT    UNIQUE,
    pass     TEXT,
    token    BLOB,
    plays    INTEGER NOT NULL
                     DEFAULT 0,
    floor    TEXT,
    letter   TEXT,
    house    TEXT,
    PRIMARY KEY (
        id AUTOINCREMENT
    )
);

CREATE TABLE Configuration (
    Id        INTEGER PRIMARY KEY AUTOINCREMENT,
    ValidHour TEXT,
    CourtId   INTEGER
);

CREATE TABLE courts (
    id          INTEGER,
    name        TEXT,
    urba_id     INTEGER,
    type                NOT NULL
                        DEFAULT 0,
    valid_times TEXT,
    PRIMARY KEY (
        id AUTOINCREMENT
    )
);

CREATE TABLE normative (
    Id     INTEGER NOT NULL
                   UNIQUE,
    Title  TEXT,
    Text   TEXT,
    UrbaId INTEGER,
    PRIMARY KEY (
        Id AUTOINCREMENT
    )
);

CREATE TABLE Scheduler (
    Id       INTEGER NOT NULL
                     PRIMARY KEY AUTOINCREMENT,
    CourtId  INTEGER,
    Time     TEXT,
    ClientId INTEGER,
    Day      TEXT,
    Duration TEXT
);


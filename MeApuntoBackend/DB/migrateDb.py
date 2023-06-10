import sqlite3
import sys
from dataDto import Normative, Client

class NormativeMigration():
    def __init__(self, conn):
        sql_text = '''SELECT * from normative;'''
        self.cur = conn.cursor()
        self.conn = conn
        self.cur.execute(sql_text)
        conn.commit()
        allNormative = self.cur.fetchall()
        self.normatives = []
        for n in allNormative:
            self.normatives.append(Normative(n[1], n[2], n[3]))

    def getAll(self):
        return self.normatives

    def insert(self, conn, normatives: list):
        for n in normatives:
            sql_text = ''' insert into normative (title, text, UrbaId) values (?,?,?);'''
            cur = conn.cursor()
            cur.execute(sql_text, (n.title, n.text, n.urba, ))
            conn.commit()

class ClientMigration():
    def __init__(self, conn):
        sql_text = '''SELECT * from clients;'''
        self.cur = conn.cursor()
        self.conn = conn
        self.cur.execute(sql_text)
        conn.commit()
        allClients = self.cur.fetchall()
        self.clients = []
        for n in allClients:
            self.clients.append(Client(n[1], n[2], n[3], n[4], n[5], n[6], n[7], n[8], n[9]))

    def getAll(self):
        return self.clients

    def insert(self, conn, clients: list):
        for n in clients:
            sql_text = ''' insert into clients (urba_id, name, username, pass, token, plays, floor, letter, house) values (?,?,?,?,?,?,?,?,?);'''
            cur = conn.cursor()
            cur.execute(sql_text, (n.urba_id, n.name, n.username, n.passw, n.token, n.plays, n.floor, n.letter, n.house, ))
            conn.commit()


DB_COPY_FROM = "fuck.db"
DB_COPY_TO = "db_production.db"

# Read first db:
conn = sqlite3.connect(DB_COPY_FROM, check_same_thread=False)
if not conn:
    print("Error connecting: ", DB_COPY_FROM)

# Migrate normative:
"""
normative = NormativeMigration(conn)
all_normatives = normative.getAll()
conn = sqlite3.connect(DB_COPY_TO, check_same_thread=False)
if not conn:
    print("Error connecting", DB_COPY_TO)
normative.insert(conn, all_normatives)
"""

# Migrate clients:
"""
clients = ClientMigration(conn)
all_clients = clients.getAll()
conn = sqlite3.connect(DB_COPY_TO, check_same_thread=False)
if not conn:
    print("Error connecting", DB_COPY_TO)
clients.insert(conn, all_clients)
"""

sql_text = '''SELECT * from clients where name like ?;'''
cur = conn.cursor()
cur.execute(sql_text, (sys.argv[1] + '%', ))
conn.commit()
client = cur.fetchall()
print("----> id", client)


# Generate new format of hours
"""
valid_hours = ["09:00","10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00"]
court_id = 15
half_hour = True

conn = sqlite3.connect(DB_COPY_TO, check_same_thread=False)
if not conn:
    print("Error connecting", DB_COPY_TO)

for h in valid_hours:
    sql_text = '''insert into Configuration (ValidHour, CourtId) values (?,?);'''
    cur = conn.cursor()
    cur.execute(sql_text, (h, court_id, ))
    if half_hour:
        new_hour = h.split(':')[0] + ':30'
        cur.execute(sql_text, (new_hour, court_id, ))
    conn.commit()
"""

# For regenerate indexes:


if __name__ == "__main__":
    pass

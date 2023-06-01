import sqlite3
from datetime import datetime
import threading

DB_COPY_FROM = "fuck.db"
DB_COPY_TO = "db_production.db"

# Read first db:
conn = sqlite3.connect(DB_COPY_FROM, check_same_thread=False)
if not conn:
    print("Error connecting")

class Normative(object):
    def __init__(self, title, text, urba):
        self.title = title
        self.text = text
        self.urba = urba

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



# Write second DB
normative = NormativeMigration(conn)
all_normatives = normative.getAll()
conn = sqlite3.connect(DB_COPY_TO, check_same_thread=False)
if not conn:
    print("Error connecting")
normative.insert(conn, all_normatives)

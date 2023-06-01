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

class Client(object):
    def __init__(self, urba_id, name, username, passw, token, plays, floor, letter, house):
        self.urba_id =urba_id 
        self.name = name 
        self.username =username 
        self.passw = passw
        self.token = token
        self.plays = plays 
        self.floor = floor 
        self.letter = letter 
        self.house = house 

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
            cur.execute(sql_text, (n.urba_id, n.name, n.username, n.token, n.passw, n.plays, n.floor, n.letter, n.house, ))
            conn.commit()


# Migrate normative:
"""
normative = NormativeMigration(conn)
all_normatives = normative.getAll()
conn = sqlite3.connect(DB_COPY_TO, check_same_thread=False)
if not conn:
    print("Error connecting")
normative.insert(conn, all_normatives)
"""

# Migrate clients:
clients = ClientMigration(conn)
all_clients = clients.getAll()
for c in all_clients:
    print(c.name)
conn = sqlite3.connect(DB_COPY_TO, check_same_thread=False)
if not conn:
    print("Error connecting")
clients.insert(conn, all_clients)
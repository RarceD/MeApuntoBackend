import sqlite3
import hashlib
import random

def generate_hash(data = str(random.randrange(0,10000000))):
    sha256_hash = hashlib.sha256(data.encode()).hexdigest()[:32]
    return sha256_hash

class UrbaClientGenerator():
    def __init__(self, conn):
        self.conn = conn
        self.cur = conn.cursor()

    def create_urba(self, urba_name, info, advance_book=3):
        sql_text = '''insert into urbas (name, info, key,advance_book, free) values (?,?,?,?,0);'''
        key = generate_hash()
        self.cur.execute(sql_text, (urba_name, info, key, advance_book))
        self.conn.commit()
        return self.get_urba_by_name(urba_name)[0]

    def get_urba_by_name(self, urba_name):
        sql_text = '''select * from urbas where name=?;'''
        self.cur.execute(sql_text, (urba_name, ))
        self.conn.commit()
        urbaFound = self.cur.fetchall()
        return urbaFound[0];

    def create_court(self, court_name, urba_id):
        sql_text = '''insert into courts (name, urba_id) values (?, ?);'''
        self.cur.execute(sql_text)
        conn.commit()

DB_TO_ADD = "db_production.db"
if __name__ == "__main__":
    conn = sqlite3.connect(DB_TO_ADD, check_same_thread=False)
    urbaGenerator = UrbaClientGenerator(conn)
    # urba_id = urbaGenerator.create_urba('TEST URBA', 'Burgos - horarios de 9:00 a 22:00')
    print(urba_id)
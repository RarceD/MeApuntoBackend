
class Normative(object):
    def __init__(self, title, text, urba):
        self.title = title
        self.text = text
        self.urba = urba


class Client(object):
    def __init__(self, urba_id, name, username, passw, token, plays, floor, letter, house):
        self.urba_id = urba_id
        self.name = name
        self.username = username
        self.passw = passw
        self.token = token
        self.plays = plays
        self.floor = floor
        self.letter = letter
        self.house = house

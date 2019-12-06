def route(start,objects):
    theroute = []
    while (start!="COM"):
        theroute.append(start)
        start=objects[start]
    return theroute
f = open("..\input\day6.txt", "r")
objects,count = {},0
for x in f: objects[x.rstrip().split(')')[1]]=x.split(')')[0]
for item in objects: count+=len(route(item,objects))
print("AnswerA: ", count)
print("AnswerB: ", len(set(route("SAN",objects)) ^ set(route("YOU",objects)))-2)
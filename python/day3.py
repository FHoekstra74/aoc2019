def Path(input):
    line = [(1,1)]
    x=y=1
    for instruction in input.split(','):
        direction = instruction[0]
        length = int(instruction[1:])
        for i in range(length):
            if direction == 'U':
                y+=1
            elif direction == 'D':
                y-=1
            elif direction == 'L':
                x-=1
            elif direction == 'R':
                x+=1
            line.append((x,y))
    return line

f = open("..\input\day3.txt", "r")
line1 = Path(f.readline())
line2 = Path(f.readline())
f.close()

intersections = set(line1).intersection(set(line2))
closest=nearest=99999

for intersec in intersections:
    dist = abs(intersec[0]-1) + abs(intersec[1]-1)
    if dist < closest and dist > 0:
        closest = dist
    steps = line1.index(intersec) + line2.index(intersec)
    if steps < nearest and steps > 0:
        nearest = steps 

print ("AnswerA:", closest)
print ("AnswerB:", nearest)

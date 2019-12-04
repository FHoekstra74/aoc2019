def ValidPassword(input):
    items=list(map(int,list(str(input))))
    twosame=False
    for i in range(6):
        if i>0:
            if (items[i - 1] == items[i]):
                 twosame = True
            if (items[i - 1] > items[i]):
                 return False
    return twosame

def ValidPassword2(input):
    items=list(map(int,list(str(input))))
    found=False
    test=[0 for i in range(10)]
    for i in range(6):
        test[items[i]-1]+=1
    for i in range(9):
        if test[i] == 2:
            return True

count1=count2=0
for i in range (158126 , 624574):
    if ValidPassword(i):
        count1+=1
        if ValidPassword2(i):
            count2+=1

print ("AnswerA:", count1)
print ("AnswerB:", count2)
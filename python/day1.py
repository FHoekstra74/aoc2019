answerA=0
answerB=0
f = open("..\input\day1.txt", "r")
for x in f:
  fuel=int(int(x)/3)-2  
  answerA=answerA+fuel
  val=fuel
  while val>0:
      answerB=answerB+val
      val=int(val/3)-2
f.close()
print ("AnswerA: " , answerA)
print ("AnswerB: " , answerB)
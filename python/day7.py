import queue 
def valid(nr,validvals):
    for c in nr:
        if int(c) not in validvals: return (False)
    return len(set(nr)) == len(nr)
def run(nums,input,output,pointer):
    opcode=0
    while opcode < 99:
        t=f"{nums[pointer[0]]:05}"
        opcode,b,c=int(t[3:]),int(t[1]),int(t[2])
        if opcode != 3 and opcode != 99: par1 = nums[nums[pointer[0] + 1]] if c == 0 else nums[pointer[0] + 1]
        if opcode != 4 and opcode != 3 and opcode != 99: par2 = nums[nums[pointer[0] + 2]] if b == 0 else nums[pointer[0] + 2]
        if opcode == 1: nums[nums[pointer[0] + 3]] = par1 + par2
        elif opcode == 2: nums[nums[pointer[0] + 3]] = par1 * par2
        elif opcode == 3: nums[nums[pointer[0] + 1]] = input.get()
        elif opcode == 4: 
            output.put(par1)
            pointer[0]+=2
            return(par1)
        elif opcode == 5: pointer[0]=par2 if par1 != 0 else pointer[0]+3
        elif opcode == 6: pointer[0]=par2 if par1 == 0 else pointer[0]+3
        elif opcode == 7: nums[nums[pointer[0] + 3]] = 1 if par1 < par2 else 0
        elif opcode == 8: nums[nums[pointer[0] + 3]] = 1 if par1 == par2 else 0
        if opcode in [1,2,7,8]: pointer[0]+=4
        elif opcode in [3,4]: pointer[0]+=2
    return(-99999)
nums,highest = [int(n) for n in open("..\input\day7.txt", "r").read().split(',')], [0] * 2
for j in range(0,100000):
    if valid(f"{j:05}",[0,1,2,3,4]) or valid(f"{j:05}",[5,6,7,8,9]):
        q,c,p,res=[],[],[],0
        for i in range (5): q.append(queue.Queue(maxsize=20))
        for i in range (5): q[i].put(int(f"{j:05}"[i]))
        for i in range (5): c.append(nums.copy())
        for i in range (5): p.append([0])
        q[0].put(0)
        while res !=-99999 or (j<5000 and res==0): 
            for i in range (5): res=run(c[i],q[i],q[i+1 if i<4 else 0],p[i])
        res=q[0].get()
        if res>highest[1 if j>5000 else 0]: highest[1 if j>5000 else 0]=res
print("AnswerA: ",highest[0],"\nAnswerB: ",highest[1])
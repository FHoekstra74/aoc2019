import queue 
from collections import defaultdict
def run(nums,input,output,pointer,pointer2):
    opcode=0
    while opcode < 99:
        t=f"{nums[pointer[0]]:05}"
        opcode,a,b,c,=int(t[3:]),int(t[0]),int(t[1]),int(t[2])
        if opcode in [1,2,7,8]: write = nums[pointer[0] + 3] if a == 0 else nums[pointer[0] + 3] + pointer2[0]
        if opcode == 3: write=nums[pointer[0] + 1] if c == 0 else nums[pointer[0] + 1] + pointer2[0]
        if opcode != 3 and opcode != 99: par1 = nums[nums[pointer[0] + 1]] if c == 0 else nums[pointer[0] + 1] if c == 1 else nums[nums[pointer[0] + 1] + pointer2[0]]
        if opcode not in [3,4,9]: par2 = nums[nums[pointer[0] + 2]] if b == 0 else nums[pointer[0] + 2] if b == 1 else nums[nums[pointer[0] + 2] + pointer2[0]]
        if opcode == 1: nums[write] = par1 + par2
        elif opcode == 2: nums[write] = par1 * par2
        elif opcode == 3:
            if input.empty(): return(-88888)
            else: nums[write] = input.get()
        elif opcode == 4: output.put(par1)
        elif opcode == 5: pointer[0]=par2 if par1 != 0 else pointer[0]+3
        elif opcode == 6: pointer[0]=par2 if par1 == 0 else pointer[0]+3
        elif opcode == 7: nums[write] = 1 if par1 < par2 else 0
        elif opcode == 8: nums[write] = 1 if par1 == par2 else 0
        elif opcode == 9: pointer2[0]= pointer2[0]+ par1
        if opcode in [1,2,7,8]: pointer[0]+=4
        elif opcode in [4,3,9]: pointer[0]+=2
    return(-99999)
nums = [int(n) for n in open("..\input\day13.txt", "r").read().split(',')]
pointer, pointer2, resA, balx, score,inputq,outputq,ddnums,res = [0],[0],0,0,0,queue.SimpleQueue(),queue.SimpleQueue(),defaultdict(int),0
for i in range(len(nums)): ddnums[i] = nums[i] if i > 0 else 2
while res!=-99999:
    res=run(ddnums,inputq,outputq,pointer,pointer2)
    while not outputq.empty():
        x, y, tile = outputq.get(),outputq.get(),outputq.get()
        if x == -1 and y == 0: score = tile
        if tile == 2: resA += 1
        elif tile == 3: curx = x 
        elif tile == 4: balx = x
    inputq.put(1 if balx > curx else -1 if balx < curx else 0)
print("AnswerA:",resA,"\nAnswerB:",score)
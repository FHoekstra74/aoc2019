def run(nums,input):
    pointer=0
    while True:
        t=f"{nums[pointer]:05}"
        opcode,a,b,c=int(t[3:]),int(t[0]),int(t[1]),int(t[2])
        if opcode in[ 1, 2, 4, 5, 6, 7, 8]:
            par1 = nums[nums[pointer + 1]] if c==0 else nums[pointer + 1]
        if opcode in[ 1, 2, 5, 6, 7, 8]:
            par2 = nums[nums[pointer + 2]] if b==0 else nums[pointer + 2]
        if opcode == 99:
            break
        elif opcode == 1:
            nums[nums[pointer + 3]] = par1 + par2
        elif opcode == 2:
            nums[nums[pointer + 3]] = par1 * par2
        elif opcode == 3:
            nums[nums[pointer + 1]] = input
        elif opcode == 4:
            if par1!=0: print(par1)
        elif opcode == 5:
            if par1 !=0: pointer = par2 
            else: pointer += 3
        elif opcode == 6:
            if par1 ==0: pointer = par2 
            else: pointer += 3
        elif opcode == 7:
            if par1 < par2: nums[nums[pointer + 3]] = 1 
            else: nums[nums[pointer + 3]] = 0
        elif opcode == 8:
            if par1 == par2 : nums[nums[pointer + 3]] = 1 
            else: nums[nums[pointer + 3]] = 0
        if opcode in [1, 2, 7, 8]:
            pointer+=4
        if opcode in [3,4]:
            pointer+=2

f = open("..\input\day5.txt", "r")
input = f.read()
f.close()
nums = [int(n) for n in input.split(',')]
print ("AnswerA: ",end='') 
run(nums.copy(),1)
print ("AnswerB: ",end='')
run(nums,5)
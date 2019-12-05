def run(nums,input):
    pointer=opcode=0
    while opcode < 99:
        t=f"{nums[pointer]:05}"
        opcode,b,c=int(t[3:]),int(t[1]),int(t[2])
        if opcode < 9: 
            par1 = nums[nums[pointer + 1]] if c == 0 else nums[pointer + 1]
            if opcode != 4: par2 = nums[nums[pointer + 2]] if b == 0 else nums[pointer + 2]
        if opcode == 1: nums[nums[pointer + 3]] = par1 + par2
        elif opcode == 2: nums[nums[pointer + 3]] = par1 * par2
        elif opcode == 3: nums[nums[pointer + 1]] = input
        elif opcode == 4 and par1 > 0: return(par1)
        elif opcode == 5: pointer=par2 if par1 != 0 else pointer+3
        elif opcode == 6: pointer=par2 if par1 == 0 else pointer+3
        elif opcode == 7: nums[nums[pointer + 3]] = 1 if par1 < par2 else 0
        elif opcode == 8: nums[nums[pointer + 3]] = 1 if par1 == par2 else 0
        if opcode in [1,2,7,8]: pointer+=4
        elif opcode in [3,4]: pointer+=2
nums = [int(n) for n in open("..\input\day5.txt", "r").read().split(',')]
print ("AnswerA: ", run(nums.copy(),1))
print ("AnswerB: ", run(nums,5))
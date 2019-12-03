def run(nums):
    pointer=0
    while True:
        opcode=nums[pointer]
        if opcode == 99:
            break
        if opcode == 1:
            nums[nums[pointer + 3]] = nums[nums[pointer + 1]] + nums[nums[pointer + 2]]
            pointer+=4
        if opcode == 2:
            nums[nums[pointer + 3]] = nums[nums[pointer + 1]] * nums[nums[pointer + 2]]
            pointer+=4
    return nums[0]

f = open("..\input\day2.txt", "r")
input = f.read()
f.close()

nums = [int(n) for n in input.split(',')]
nums[1], nums[2] = 12,2

print ("AnswerA:", run(nums))

noun = verb = 1
while True:
    nums = [int(n) for n in input.split(',')]
    nums[1], nums[2] = noun, verb

    if (run(nums) == 19690720):
        break
    else:
        if noun == 99:
            verb+=1
            noun=1
        else:
            noun+=1
print ("AnswerB:", noun * 100 + verb)
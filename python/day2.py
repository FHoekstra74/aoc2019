def run(nums):
    pointer=0
    while True:
        opcode=nums[pointer]
        if opcode == 99:
            break
        if opcode == 1:
            nums[nums[pointer + 3]] = nums[nums[pointer + 1]] + nums[nums[pointer + 2]]
            pointer = pointer + 4
        if opcode == 2:
            nums[nums[pointer + 3]] = nums[nums[pointer + 1]] * nums[nums[pointer + 2]]
            pointer = pointer + 4
    return nums[0]

f = open("..\input\day2.txt", "r")
input = f.read()
f.close()

nums = [int(n) for n in input.split(',')]
nums[1] = 12
nums[2] = 2

print ("AnswerA:", run(nums))

noun=1
verb=1
while True:
    nums = [int(n) for n in input.split(',')]
    nums[1]=noun
    nums[2]=verb

    if (run(nums) == 19690720):
        break
    else:
        if noun == 99:
            verb=verb+1
            noun=1
        else:
            noun=noun+1
print ("AnswerB:", noun * 100 + verb)
input = open("..\input\day8.txt", "r").readline().rstrip()
min,res,width,height = 99999,0,25,6
output = ['2'] * (width*height)
layers = [input[i:i+(width*height)] for i in range(0, len(input), width*height)]
for layer in layers:
    if layer.count('0') < min:
        min=layer.count('0')
        res=layer.count('1') * layer.count('2')
    for x in range(width*height):
        if output[x]=='2':
            output[x]=layer[x]
print ("AnsewerA",res)
for line in range(height):
    p=''
    for c in output[(line*width):((line+1)*width)]:
        p = p + ' ' if c == '0' else p + '#'
    print(p)
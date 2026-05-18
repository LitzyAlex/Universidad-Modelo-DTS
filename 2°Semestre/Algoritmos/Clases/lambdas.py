
def square(element):
    return element**2


op=lambda e: e**2

#result=square(2)
#print(result)

result=op(2)
print(result)

#****************************
print('\n****************\n')
#****************************

def test_digit(digit,condition):
    return condition(digit)

print(test_digit(5,lambda d: d==10))
print(test_digit(10,lambda d: d==10))
print(test_digit(12,lambda d: d==10))

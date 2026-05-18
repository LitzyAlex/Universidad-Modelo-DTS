#****************************
print('\n****************\n')
#****************************

data = [1,7,-3,0,-6,2,18,9]

print(type(data))

print(data[2])
print(data[-1]) #Verlo desde atras, el ultimo es -1

#****************************
print('\n****************\n')
#****************************

for e in data:
    print(e)

#****************************
print('\n****************\n')
#****************************

for i in range(len(data)): #Con indices
    print(data[i])

#****************************
print('\n****************\n')
#****************************

data.sort()
print(data)

#****************************
print('\n****************\n')
#****************************

data.sort(key= lambda e:abs(e)) #Decirle como debe percibir los elementos
print(data)

#****************************
print('\n****************\n')
#****************************

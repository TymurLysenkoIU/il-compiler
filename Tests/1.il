var a: integer is ((((((((1))))))))
var b is 10; var c is true
var d is 2 + 2 * 2

// Identifiers are alphanumeric words with allowed low-dash not in the beginning, starting with a letter. Uppercase for Type, lowercase for var

// Assign to e real value
var e is ((a + b) + 3) / (30 % 12)

// Assign a boolean expression: real > integer => true
var f is e > a

type IntArr10 is array [10] integer
type IntArr5 is array [5] integer

type User is record
	var age: integer
	var isCool: boolean
end

type Struct is record
	var key: integer
	var zumba: real
	var isOkay: boolean
	var deep: record
		var down: boolean
	end
	var users: array[2] User
end

// Example of manual record filling
var g: Struct
g.key := 1
g.zumba := 1.12
g.isOkay := true
g.deep.down := true
g.users[1].age := 15
g.user[1].isCool := false
g.users[2].age := 24
g.user[2].isCool := true

// Example of manual array filling
var h: IntArr5
h[1] := 1
h[2] := 2
h[3] := 3
h[4] := 4
h[5] := 5

// Square function
routine square (num: real) : real is
	return num * num
end


// A void that fills the array10 by reference
// with numbers 1..10
routine fillArray10 (arr: IntArr10) is
	for i in 1..10 loop
		arr[i] := i
	end
end

// Return the squares of reversed even numbers of IntArr10, so result becomes IntArr5: [100, 64, 36, 16, 4, 0]
routine main () : IntArr5 is
	van arr: IntArr10s
	fillTheArray(arr)

	var newArr: IntArr5
	for i in reverse 1..10 loop
		var value is arr[i]
		if (value % 2 = 0) then
			newArr[(10 - i) / 2] := square(i)
		end
	end
	return newArr
end
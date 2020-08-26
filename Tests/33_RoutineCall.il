routine SecondRoutine( FirstArgument : integer, SecondArgument : integer) : integer is
	return FirstArgument * SecondArgument
end

routine FirstRoutine() is 
	var FirstIdentifier := SecondRoutine( 10, 20 )
end
routine SecondRoutine( FirstArgument : integer, SecondArgument : integer) : integer is
	var FirstIdentifier = FirstArgument * SecondArgument
	return FirstIdentifier
end

routine FirstRoutine() is 
	var FirstIdentifier := SecondRoutine( 10, 20 )
end
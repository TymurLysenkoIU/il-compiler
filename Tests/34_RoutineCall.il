routine SecondRoutine( FirstArgument : integer, SecondArgument : integer) : real is
	return FirstArgument * SecondArgument
end

routine FirstRoutine() is 
	var FirstIdentifier := SecondRoutine( 10, 20 )
end
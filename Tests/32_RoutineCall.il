routine SecondRoutine( FirstArgument : integer) : integer is
	return FirstArgument
end

routine FirstRoutine() is 
	FirstIdentifier := SecondRoutine( 10 )
end
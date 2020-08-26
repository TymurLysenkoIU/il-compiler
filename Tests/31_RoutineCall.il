routine SecondRoutine() : integer is
	return 5
end

routine FirstRoutine() is 
	FirstIdentifier := SecondRoutine()
end
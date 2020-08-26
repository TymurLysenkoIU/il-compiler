routine SecondRoutine() : integer is
	return 5
end

routine FirstRoutine() is 
	var FirstIdentifier := SecondRoutine()
end
routine FirstRoutine() is 
		
	var Array5_int : array [5] integer
	var Array5_real : array [5] real
	var Array5_bool : array [5] boolean

	for FirstIdentifier in 1..5 loop
		Array5_int [ FirstIdentifier ] := FirstIdentifier * FirstIdentifier
		Array5_real [ FirstIdentifier ] := FirstIdentifier * FirstIdentifier
		Array5_bool [ FirstIdentifier ] := true
	end

end
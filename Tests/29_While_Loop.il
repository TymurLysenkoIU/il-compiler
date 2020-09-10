routine FirstRoutine() is 
		
	var Array5_int : array [5] integer
	var Array5_real : array [5] real
	var Array5_bool : array [5] boolean

	var FirstIdentifier is 5

	while FirstIdentifier /= 0 loop
		Array5_int [ FirstIdentifier ] := FirstIdentifier 
		Array5_real [ FirstIdentifier ] := FirstIdentifier 
		Array5_bool [ FirstIdentifier ] := true

		FirstIdentifier := FirstIdentifier - 1

	end

end
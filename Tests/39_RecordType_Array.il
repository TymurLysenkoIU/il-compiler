type FirstType is record
	var FirstIdentifier is 10;
	var SecondIdentifier is 10.0;
end

routine FirstRoutine() is 
	var FirstIdentifier : array[5] FirstType
	
	for SecondIdentifier in 1..5 loop
		FirstIdentifier [ SecondIdentifier ].FirstIdentifier := 1
		FirstIdentifier [ SecondIdentifier ].SecondIdentifier := 1.0
	end

end
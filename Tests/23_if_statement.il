routine FirstRoutine() is 
	
	var FirstIdentifier is false;
	var SecondIdentifier is false;


	if FirstIdentifier = 2 then
		ThirdIdentifier := 1;
		ThirdIdentifier := ThirdIdentifier * 5;
	else
		ThirdIdentifier := 2;
		ThirdIdentifier := ThirdIdentifier * 3;
	end

	if SecondIdentifier = 1.5 then
		FourthIdentifier := 1;
		FourthIdentifier := ThirdIdentifier * 5;
	else
		FourthIdentifier := 2;
		FourthIdentifier := ThirdIdentifier * 3;
	end

end
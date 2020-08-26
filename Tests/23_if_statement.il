routine FirstRoutine() is 
	
	var FirstIdentifier is false;
	var SecondIdentifier is false;


	if FirstIdentifier = 2 then
		var ThirdIdentifier := 1;
		ThirdIdentifier := ThirdIdentifier * 5;
	else
		var ThirdIdentifier := 2;
		ThirdIdentifier := ThirdIdentifier * 3;
	end

	if SecondIdentifier = 1.5 then
		var FourthIdentifier := 1;
		FourthIdentifier := ThirdIdentifier * 5;
	else
		var FourthIdentifier := 2;
		FourthIdentifier := ThirdIdentifier * 3;
	end

end
routine FirstRoutine() is 
		 
	var FirstIdentifier is false;
	var SecondIdentifier is flase;


	if FirstIdentifier = true 
	then
		var ThirdIdentifier := 1;
		ThirdIdentifier := ThirdIdentifier * 5;
	else
		var ThirdIdentifier := 2;
		ThirdIdentifier := ThirdIdentifier * 3;
	end

	if SecondIdentifier = false 
	then
		var FourthIdentifier := 1;
		FourthIdentifier := ThirdIdentifier * 5;
	else
		var FourthIdentifier := 2;
		FourthIdentifier := ThirdIdentifier * 3;
	end

end
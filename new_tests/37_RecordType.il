type FirstType is record
	var FirstIdentifier is 10;
	var SecondIdentifier is 10.0;
end

routine FirstRoutine() is 
	var FirstIdentifier : FirstType
	var SeconIdentifier := FirstIdentifier.FirstIdentifier
	var ThirdIdentifier := FirstIdentifier.SecondIdentifier
	var FourthIdentifier := FirstIdentifier
	FourthIdentifier.FirstIdentifier := 1
	FourthIdentifier.SecondIdentifier := 1.0
end
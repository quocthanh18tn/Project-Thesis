#include "KMotionDef.h"

int main()
{
	FPGA(STEP_PULSE_LENGTH_ADD) = 35 + 0x80;  // set polarity (pulse the Step High) and pulse length to 4us,  


	ch0->InputMode=NO_INPUT_MODE;
	ch0->OutputMode=STEP_DIR_MODE;
	ch0->Vel=500;
	ch0->Accel=200000;
	ch0->Jerk=4e+006;
	ch0->P=0;
	ch0->I=0.01;
	ch0->D=0;
	ch0->FFAccel=0;
	ch0->FFVel=0;
	ch0->MaxI=200;
	ch0->MaxErr=1e+006;
	ch0->MaxOutput=200;
	ch0->DeadBandGain=1;
	ch0->DeadBandRange=0;
	ch0->InputChan0=0;
	ch0->InputChan1=0;
	ch0->OutputChan0=8;
	ch0->OutputChan1=0;
	ch0->MasterAxis=-1;
	ch0->LimitSwitchOptions=0x11f;
	ch0->LimitSwitchNegBit=13;
	ch0->LimitSwitchPosBit=12;
	//ch0->LimitSwitchOptions=0x0;
	ch0->SoftLimitPos=1e+030;
	ch0->SoftLimitNeg=-1e+030;
	ch0->InputGain0=1;
	ch0->InputGain1=1;
	ch0->InputOffset0=0;
	ch0->InputOffset1=0;
	ch0->OutputGain=1;
	ch0->OutputOffset=0;
	ch0->SlaveGain=1;
	ch0->BacklashMode=BACKLASH_OFF;
	ch0->BacklashAmount=0;
	ch0->BacklashRate=0;
	ch0->invDistPerCycle=1;
	ch0->Lead=0;
	ch0->MaxFollowingError=1000000000;
	ch0->StepperAmplitude=20;

	ch0->iir[0].B0=1;
	ch0->iir[0].B1=0;
	ch0->iir[0].B2=0;
	ch0->iir[0].A1=0;
	ch0->iir[0].A2=0;

	ch0->iir[1].B0=1;
	ch0->iir[1].B1=0;
	ch0->iir[1].B2=0;
	ch0->iir[1].A1=0;
	ch0->iir[1].A2=0;

	ch0->iir[2].B0=1;
	ch0->iir[2].B1=0;
	ch0->iir[2].B2=0;
	ch0->iir[2].A1=0;
	ch0->iir[2].A2=0;
	EnableAxis(0); // enable the Axis at the current encoder position
	
	ch1->InputMode=NO_INPUT_MODE;
	ch1->OutputMode=STEP_DIR_MODE;
	ch1->Vel=500;
	ch1->Accel=200000;
	ch1->Jerk=4e+006;
	ch1->P=0;
	ch1->I=0.01;
	ch1->D=0;
	ch1->FFAccel=0;
	ch1->FFVel=0;
	ch1->MaxI=200;
	ch1->MaxErr=1e+006;
	ch1->MaxOutput=200;
	ch1->DeadBandGain=1;
	ch1->DeadBandRange=0;
	ch1->InputChan0=0;
	ch1->InputChan1=0;
	ch1->OutputChan0=9;
	ch1->OutputChan1=0;
	ch1->MasterAxis=-1;
	ch1->LimitSwitchOptions=0x11f;
	ch1->LimitSwitchNegBit=15;
	ch1->LimitSwitchPosBit=14;
	//ch1->LimitSwitchOptions=0x0;
	ch1->SoftLimitPos=1e+030;
	ch1->SoftLimitNeg=-1e+030;
	ch1->InputGain0=1;
	ch1->InputGain1=1;
	ch1->InputOffset0=0;
	ch1->InputOffset1=0;
	ch1->OutputGain=1;
	ch1->OutputOffset=0;
	ch1->SlaveGain=1;
	ch1->BacklashMode=BACKLASH_OFF;
	ch1->BacklashAmount=0;
	ch1->BacklashRate=0;
	ch1->invDistPerCycle=1;
	ch1->Lead=0;
	ch1->MaxFollowingError=1000000000;
	ch1->StepperAmplitude=20;

	ch1->iir[0].B0=1;
	ch1->iir[0].B1=0;
	ch1->iir[0].B2=0;
	ch1->iir[0].A1=0;
	ch1->iir[0].A2=0;

	ch1->iir[1].B0=1;
	ch1->iir[1].B1=0;
	ch1->iir[1].B2=0;
	ch1->iir[1].A1=0;
	ch1->iir[1].A2=0;

	ch1->iir[2].B0=1;
	ch1->iir[2].B1=0;
	ch1->iir[2].B2=0;
	ch1->iir[2].A1=0;
	ch1->iir[2].A2=0;
	EnableAxis(1);
	
	DefineCoordSystem(0,1,-1,-1); // define axis chan numbers to useas x,y,z,a (set -1 to disable)
	
	while(1);
	
	return 0;
}
	
using System;
using System.Collections;

namespace TSBTool
{
	public enum TSBPlayer
	{
		QB1=0,QB2,RB1,RB2,RB3,RB4,WR1,WR2,WR3,WR4,
		TE1,TE2,C,LG,RG,LT,RT,
		RE,NT,LE,ROLB,RILB,LILB,LOLB,RCB,LCB,FS,SS,
		K,P
	}

	public interface ITecmoTool
	{
		byte[] OutputRom{get; set;}

		bool ShowOffPref {get; set;}

		ArrayList Errors {get; set; }

		string GetKey();

		string GetAll();

		string GetPlayerStuff(bool jerseyNumbers,bool names,bool faces,bool abilities,bool simData);

		string GetSchedule();

		void SaveRom(string filename);

		void ApplySet(string line);

		void SetPlaybook(string team, string runs, string passes);

		bool ApplyJuice(int week,int amount);

		void SetTeamSimData(string team, byte data);

		bool SetTeamSimOffensePref(string team, int val);

		void SetTeamOffensiveFormation(string team, string formation);

		void SetYear(string year);

		bool IsValidPosition(string position);

		void SetFace(string team, string position, int face);

		void InsertPlayer(string currentTeam,string pos,string fname,string lname,byte jerseyNumber);

		void SetQBAbilities(string team, 
			string qb, 
			int runningSpeed, 
			int rushingPower, 
			int maxSpeed,
			int hittingPower,
			int passingSpeed,
			int passControl,
			int accuracy, 
			int avoidPassBlock
			);

		void SetQBSimData(string team, string pos, int[] data);

		void SetSkillPlayerAbilities(string team, 
			string pos, 
			int runningSpeed, 
			int rushingPower, 
			int maxSpeed,
			int hittingPower,
			int ballControl,
			int receptions
			);

		void SetSkillSimData(string team, string pos, int[] data);

		void SetOLPlayerAbilities(string team, 
			string pos, 
			int runningSpeed, 
			int rushingPower, 
			int maxSpeed,
			int hittingPower );

		void SetDefensivePlayerAbilities(string team, 
			string pos, 
			int runningSpeed, 
			int rushingPower, 
			int maxSpeed,
			int hittingPower,
			int passRush,
			int interceptions
			);

		void SetDefensiveSimData(string team, string pos, int[] data);

		void SetKickPlayerAbilities(string team, 
			string pos, 
			int runningSpeed, 
			int rushingPower, 
			int maxSpeed,
			int hittingPower,
			int kickingAbility,
			int avoidKickBlock
			);
		void SetPuntingSimData(string team, int data);

		void SetKickingSimData(string team, int data);

		void SetKickReturner(string team, string position);

		void SetPuntReturner(string team, string position);

		ArrayList ApplySchedule( ArrayList scheduleList );

		void SetReturnTeam(string team, string pos0, string pos1, string pos2);

		bool Init(string fileName);

		void SetHomeUniform(string team, string colorString);

		void SetAwayUniform(string team, string colorString);
		
		string GetGameUniform(string team);

		void SetUniformUsage(string team, string usage);

		string GetUniformUsage(string team);

		void SetDivChampColors(string team, string colorString);

		void SetConfChampColors(string team, string colorString);

		string GetDivChampColors(string team );

		string GetConfChampColors(string team );

		string GetChampColors(string team);


	}
}
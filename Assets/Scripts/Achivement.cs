using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Achivement {
	public bool mission1_accomplished;
	public bool mission2_accomplished;
	public bool mission3_accomplished;

	public Achivement(bool mission1_accomplished, bool mission2_accomplished, bool mission3_accomplished)
    {
		this.mission1_accomplished = mission1_accomplished;
		this.mission2_accomplished = mission2_accomplished;
		this.mission3_accomplished = mission3_accomplished;
    }

    public Achivement()
    {
		this.mission1_accomplished = false;
		this.mission2_accomplished = false;
		this.mission3_accomplished = false;
    }
}

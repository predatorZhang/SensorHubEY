﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface INKVibratingCurve
    {
        void insert(NKVibratingCurveInfo vibratingCurve);
    }
}

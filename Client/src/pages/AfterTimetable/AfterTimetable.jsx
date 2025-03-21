import React, { useState } from 'react';
import { RefreshCw, Check } from 'lucide-react';
import Navbar from '../../components/Navbar';

const timeSlots = [
  "9:00 AM - 9:45 AM",
  "10:00 AM - 10:45 AM",
  "11:00 AM - 11:45 AM",
  "12:00 PM - 12:45 PM",
  "1:30 PM - 2:15 PM",
  "2:30 PM - 3:15 PM"
];

const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

const mockSchedule = {
  "Monday": [
    { subject: "Physics", teacher: "Frank_Physics" },
    { subject: "Chemistry", teacher: "Henry_Chem" },
    { subject: "History", teacher: "Eva_History" },
    { subject: "Science", teacher: "Charlie_Sci" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" }
  ],
  "Tuesday": [
    { subject: "History", teacher: "Eva_History" },
    { subject: "Physics", teacher: "Frank_Physics" },
    { subject: "Biology", teacher: "Grace_Biology" },
    { subject: "Science", teacher: "Charlie_Sci" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" }
  ],
  "Wednesday": [
    { subject: "Physics", teacher: "Frank_Physics" },
    { subject: "Science", teacher: "Charlie_Sci" },
    { subject: "Biology", teacher: "Grace_Biology" },
    { subject: "History", teacher: "Eva_History" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" }
  ],
  "Thursday": [
    { subject: "History", teacher: "Eva_History" },
    { subject: "Chemistry", teacher: "Henry_Chem" },
    { subject: "Science", teacher: "Charlie_Sci" },
    { subject: "Physics", teacher: "Frank_Physics" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" }
  ],
  "Friday": [
    { subject: "", teacher: "" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" }
  ],
  "Saturday": [
    { subject: "", teacher: "" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" },
    { subject: "", teacher: "" }
  ]
};

const AfterTimetable = () => {
  const [isConfirmed, setIsConfirmed] = useState(false);

  const handleRegenerate = () => {
    setIsConfirmed(false);
  };

  const handleConfirm = () => {
    setIsConfirmed(true);
  };

  return (
    <>
    <Navbar role={'organization'}/>
    <div className="min-h-screen bg-black p-6">
      <div className="max-w-[95%] mx-auto">
        <div className="bg-zinc-900 rounded-xl overflow-hidden">
          <div className="p-4 border-b border-white/10 flex justify-between items-center">
            <h1 className="text-xl font-semibold text-white">Class X - A Weekly Schedule</h1>
            <div className="flex gap-4">
              <button
                onClick={handleRegenerate}
                disabled={isConfirmed}
                className={`flex items-center gap-2 px-4 py-2 rounded-lg font-medium transition-all duration-300 
                  ${isConfirmed 
                    ? 'bg-zinc-700 cursor-not-allowed' 
                    : 'bg-zinc-800 hover:bg-zinc-700 active:scale-95'}`}
              >
                <RefreshCw className="w-4 h-4 text-white" />
                <span className="text-white">Regenerate</span>
              </button>
              <button
                onClick={handleConfirm}
                disabled={isConfirmed}
                className={`flex items-center gap-2 px-4 py-2 rounded-lg font-medium transition-all duration-300 
                  ${isConfirmed 
                    ? 'bg-green-600 cursor-not-allowed' 
                    : 'bg-zinc-800 hover:bg-zinc-700 active:scale-95'}`}
              >
                <Check className="w-4 h-4 text-white" />
                <span className="text-white">{isConfirmed ? 'Confirmed' : 'Confirm'}</span>
              </button>
            </div>
          </div>

          <div className="p-4 overflow-x-auto">
            <div className="min-w-[900px]">
              <table className="w-full border-collapse">
                <thead>
                  <tr>
                    <th className="bg-zinc-800 p-4 text-white font-medium border-b border-r border-white/10 min-w-[150px] text-center">
                      Time Slot
                    </th>
                    {days.map(day => (
                      <th key={day} className="bg-zinc-800 p-4 text-white font-medium border-b border-r border-white/10 min-w-[150px] text-center">
                        {day}
                      </th>
                    ))}
                  </tr>
                </thead>
                <tbody>
                  {timeSlots.map((timeSlot, index) => (
                    <tr key={index} className="group">
                      <td className="bg-zinc-800/50 p-4 text-white border-r border-b border-white/10 font-medium text-center">
                        {timeSlot}
                      </td>
                      {days.map(day => (
                        <td key={day} className="p-3 border-r border-b border-white/10 group-hover:bg-zinc-800/30 transition-colors duration-200 text-white">
                          {mockSchedule[day][index].subject ? (
                            <div className="space-y-2 transition-all duration-200 hover:translate-y-[-2px] flex flex-col items-center">
                              <div className="text-white font-medium">
                                {mockSchedule[day][index].subject}
                              </div>
                              <div className="text-white/70 text-sm">
                                {mockSchedule[day][index].teacher}
                              </div>
                            </div>
                          ):
                          (<div className='space-y-2 transition-all duration-200 hover:translate-y-[-2px] flex flex-col items-center'>--</div>)
                        }
                        </td>
                      ))}
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
    </>
  );
}

export default AfterTimetable;
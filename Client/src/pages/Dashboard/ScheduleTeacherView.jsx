import { React, useMemo } from 'react'
import { Calendar, AlertTriangle, Clock, GraduationCap, UserX } from 'lucide-react'



const ScheduleTeacherView = ({ selectedDay, mockTeacherSchedule, selectedDate, currentDate, days, absentClasses, setConfirmDialog }) => {
    // Get current date information


    // Check if selected day is in the past
    const isDayInPast = useMemo(() => {
        const currentDayIndex = days.indexOf(currentDate.currentDay);
        const selectedDayIndex = days.indexOf(selectedDay);
        return selectedDayIndex < currentDayIndex;
    }, [selectedDay, currentDate.currentDay]);

    const handleAbsentClick = (scheduleKey, isAlreadyAbsent) => {
        if (isDayInPast) {
            return; // Don't allow marking absent for past days
        }
        setConfirmDialog({
            isOpen: true,
            scheduleKey,
            isUnmarking: isAlreadyAbsent
        });
    };
    return (
        <div className="bg-zinc-800 rounded-xl border border-white/10">
            <div className="p-4 border-b border-white/10">
                <div className="flex flex-col gap-2">
                    <h3 className="text-lg font-semibold text-white">Schedule for {selectedDay}</h3>
                    <div className="flex items-center gap-2 text-white/70">
                        <Calendar className="h-4 w-4" />
                        <span className="text-sm">{selectedDate}</span>
                    </div>
                    {isDayInPast && (
                        <div className="mt-2 text-yellow-500/90 text-sm flex items-center gap-2">
                            <AlertTriangle className="h-4 w-4" />
                            <span>Cannot mark absences for past days</span>
                        </div>
                    )}
                </div>
            </div>
            <div className="p-4 space-y-4">
                {mockTeacherSchedule[selectedDay]?.map((schedule, index) => {
                    const scheduleKey = `${selectedDay}-${index}`;
                    const isAbsent = absentClasses.has(scheduleKey);

                    return (
                        <div
                            key={index}
                            className={`flex items-center gap-4 bg-zinc-900 p-4 rounded-lg border border-white/10 transition-colors ${isAbsent ? 'bg-red-900/20 border-red-500/30' : 'hover:bg-zinc-900/70'
                                }`}
                        >
                            <div className="flex-shrink-0">
                                <Clock className="h-5 w-5 text-white/70" />
                            </div>
                            <div className="flex-1">
                                <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-2">
                                    <div className="space-y-1">
                                        <div className="flex items-center gap-2">
                                            <h4 className="text-white font-medium">{schedule.subject}</h4>
                                            <span className="text-white/50">•</span>
                                            <div className="flex items-center gap-1.5 text-white/70">
                                                <GraduationCap className="h-4 w-4" />
                                                <span>{schedule.class}</span>
                                            </div>
                                        </div>
                                        <p className="text-white/50 text-sm">{schedule.duration}</p>
                                    </div>
                                    <div className="flex items-center gap-4">
                                        <button
                                            onClick={() => handleAbsentClick(scheduleKey, isAbsent)}
                                            disabled={isDayInPast}
                                            className={`flex items-center gap-2 px-3 py-1.5 rounded-lg text-sm font-medium transition-all ${isAbsent
                                                ? 'bg-red-500/20 text-red-300 hover:bg-red-500/30'
                                                : isDayInPast
                                                    ? 'bg-zinc-700/20 text-white/50 cursor-not-allowed'
                                                    : 'bg-zinc-700/50 text-white hover:bg-zinc-700'
                                                }`}
                                        >
                                            <UserX className="h-4 w-4" />
                                            {isAbsent ? 'Marked Absent' : 'Mark Absent'}
                                        </button>
                                        <p className="text-white font-medium">{schedule.time}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    );
                })}
            </div>
        </div>
    )
};
export default ScheduleTeacherView

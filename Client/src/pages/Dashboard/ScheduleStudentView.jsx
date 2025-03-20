import React from 'react'
import { Clock, GraduationCap } from 'lucide-react'
const ScheduleStudentView = ({ mockWeekSchedule, selectedDay, convertToSimpleTime }) => (
    <div className="bg-zinc-800 rounded-xl border border-white/10">
        <div className="p-4 border-b border-white/10 flex justify-between">
            <h3 className="text-lg font-semibold text-white">Schedule for {selectedDay}</h3>
            <h3 className="text-lg font-semibold text-white">Break at {2} PM</h3>
        </div>
        <div className="p-4 space-y-4">
            {!mockWeekSchedule[selectedDay] && <p className='text-white font-bold'>No Scheduling</p>}
            {mockWeekSchedule[selectedDay]?.map((schedule, index) => (
                <div key={index} className="flex items-center gap-4 bg-zinc-900 p-4 rounded-lg border border-white/10 hover:bg-zinc-900/70 transition-colors">
                    <div className="flex-shrink-0">
                        <Clock className="h-5 w-5 text-white/70" />
                    </div>
                    <div className="flex-1">
                        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-2">
                            <div className="space-y-1">
                                <div className="flex items-center gap-2">
                                    <h4 className="text-white font-medium">{schedule.subject}</h4>
                                    <span className="text-white/50">•</span>
                                </div>
                                <div className='flex items-center gap-2'>
                                    <p className="text-white font-medium">{schedule.teacher}</p>
                                    <strong className="text-white/50 text-sm">-</strong>
                                    <p className="text-white/50 text-sm">{60} minutes</p>
                                </div>
                            </div>
                            <div className="sm:text-right">
                                <p className="text-white font-medium">{convertToSimpleTime(schedule.startTime)}</p>
                            </div>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    </div>
);
export default ScheduleStudentView
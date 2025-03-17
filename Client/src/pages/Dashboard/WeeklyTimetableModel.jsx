import React from 'react'
import { X } from 'lucide-react'

const timeSlots = [
    "9:00 AM - 9:45 AM",
    "10:00 AM - 10:45 AM",
    "11:00 AM - 11:45 AM",
    "12:00 PM - 12:45 PM",
    "1:30 PM - 2:15 PM",
    "2:30 PM - 3:15 PM"
];
const WeeklyTimetableModel = ({ timetable, onClose, isOpen, days, mockWeekSchedule }) => {
    return (
        <div
            className={`fixed inset-0 bg-black/50 flex items-center justify-center z-50 transition-opacity duration-300 ${isOpen ? 'opacity-100' : 'opacity-0 pointer-events-none'
                }`}
            onClick={(e) => e.target === e.currentTarget && onClose()}
        >
            <div
                className={`bg-zinc-900 rounded-xl w-[95%] max-w-7xl max-h-[90vh] overflow-hidden transition-all duration-300 ${isOpen ? 'scale-100 translate-y-0' : 'scale-95 translate-y-4'
                    }`}
            >
                <div className="sticky top-0 z-10 bg-zinc-900 border-b border-white/10 p-4 flex justify-between items-center">
                    <h2 className="text-xl font-semibold text-white">
                        {timetable.className} - {timetable.division} Weekly Schedule
                    </h2>
                    <button
                        onClick={onClose}
                        className="p-2 hover:bg-zinc-800 rounded-full transition-colors"
                    >
                        <X className="text-white" size={24} />
                    </button>
                </div>
                <div className="p-4 md:p-6 overflow-x-auto">
                    <div className="min-w-[900px]">
                        <table className="w-full border-collapse bg-zinc-900 rounded-lg overflow-hidden">
                            <thead>
                                <tr>
                                    <th className="bg-zinc-800 p-4 text-white text-left font-medium border-b border-r border-white/10 min-w-[150px]">
                                        Time Slot
                                    </th>
                                    {days.map(day => (
                                        <th key={day} className="bg-zinc-800 p-4 text-white text-left font-medium border-b border-r border-white/10 min-w-[150px]">
                                            {day}
                                        </th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {timeSlots.map((timeSlot, index) => (
                                    <tr key={index} className="group">
                                        <td className="bg-zinc-800/50 p-4 text-white border-r border-b border-white/10 font-medium">
                                            {timeSlot}
                                        </td>
                                        {days.map(day => (
                                            <td key={day} className="p-4 border-r border-b border-white/10 group-hover:bg-zinc-800/30 transition-colors duration-200">
                                                {mockWeekSchedule[day]?.[index] && (
                                                    <div className="space-y-2 transition-all duration-200 hover:translate-y-[-2px]">
                                                        <div className="text-white font-medium">
                                                            {mockWeekSchedule[day][index].subject}
                                                        </div>
                                                        <div className="text-white/70 text-sm flex flex-col gap-1">
                                                            <span>{mockWeekSchedule[day][index].teacher}</span>
                                                            <span className="text-white/50 text-xs">
                                                                {mockWeekSchedule[day][index].duration}
                                                            </span>
                                                        </div>
                                                    </div>
                                                )}
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
    )
}
export default WeeklyTimetableModel
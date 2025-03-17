import React from 'react'

const OrganizationView = ({ mockTimetables, setSelectedTimetable, setIsModalOpen }) => {
    const handleTimetableClick = (timetable) => {
        setSelectedTimetable(timetable);
        setIsModalOpen(true);
    };
    return (
        <div className="bg-zinc-800 rounded-xl border border-white/10">
            <div className="p-4 border-b border-white/10">
                <h3 className="text-lg font-semibold text-white">Created Timetables</h3>
            </div>
            <div className="p-4 grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
                {mockTimetables.map((timetable) => (
                    <div
                        key={timetable.id}
                        className="bg-zinc-900 p-4 rounded-lg border border-white/10 cursor-pointer hover:bg-zinc-950 transition-all duration-200 hover:scale-[1.02] hover:shadow-xl"
                        onClick={() => handleTimetableClick(timetable)}
                    >
                        <div className="flex flex-col gap-2">
                            <div>
                                <h4 className="text-white font-medium">{timetable.className} - {timetable.division}</h4>
                                <p className="text-white/70 text-sm">Created on {timetable.createdAt}</p>
                            </div>
                            <div className="flex justify-between items-center pt-2 border-t border-white/10">
                                <p className="text-white text-sm">{timetable.totalSubjects} Subjects</p>
                                <p className="text-white/70 text-sm">{timetable.totalTeachers} Teachers</p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    )
};

export default OrganizationView

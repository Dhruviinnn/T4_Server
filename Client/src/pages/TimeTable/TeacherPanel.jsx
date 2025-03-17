import React from 'react'
import { X, Check } from "lucide-react";
import { motion } from 'framer-motion'

const TeacherPanel = ({ selectedTeachers, selectingSecondTeacher, organizationTeachers, setIsTeacherPanelOpen, setSelectingSecondTeacher, setSelectedTeachers }) => {
    const getTeacherSelectionStatus = (teacherId) => {
        const index = selectedTeachers.indexOf(teacherId);
        if (index === -1) return null;
        return index + 1;
    };
    const handleTeacherSelect = (teacherId) => {
        const currentIndex = selectedTeachers.indexOf(teacherId);
        if (currentIndex !== -1) {
            const newSelectedTeachers = [...selectedTeachers];
            newSelectedTeachers[currentIndex] = "";
            setSelectedTeachers(newSelectedTeachers);
        } else {
            const newSelectedTeachers = [...selectedTeachers];
            if (selectingSecondTeacher) {
                if (teacherId === selectedTeachers[0]) {
                    toast.error('Cannot select the same teacher twice');
                    return;
                }
                newSelectedTeachers[1] = teacherId;
            } else {
                newSelectedTeachers[0] = teacherId;
            }
            setSelectedTeachers(newSelectedTeachers);
        }
        setIsTeacherPanelOpen(false);
        setSelectingSecondTeacher(false);
    };
    return (
        <motion.div
            initial={{ opacity: 0, scale: 0.95 }}
            animate={{ opacity: 1, scale: 1 }}
            exit={{ opacity: 0, scale: 0.95 }}
            transition={{ duration: 0.2 }}
            className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center p-4"
        >
            <div className="bg-zinc-900 rounded-xl shadow-2xl border border-white/10 p-6 w-full max-w-4xl">
                <div className="flex justify-between items-center mb-4">
                    <h3 className="text-white text-lg font-semibold">
                        Select {selectingSecondTeacher ? "Second Teacher" : "Teacher"}
                    </h3>
                    <button
                        onClick={() => setIsTeacherPanelOpen(false)}
                        className="text-white/70 hover:text-white transition-colors"
                    >
                        <X className="h-5 w-5" />
                    </button>
                </div>
                <div className={`grid grid-cols-1 sm:grid-cols-${(Math.floor(organizationTeachers.teachers.length / 10))} gap-2`}>
                    {organizationTeachers.teachers.map(teacher => (
                        <button
                            key={teacher.teacher_id}
                            onClick={() => handleTeacherSelect(teacher.teacher_id)}
                            className={`w-full flex items-center justify-between bg-zinc-800 text-white rounded-lg px-4 py-3 hover:bg-zinc-700 transition-colors ${getTeacherSelectionStatus(teacher.teacher_id) ? 'bg-white text-black hover:bg-white' : ''
                                }`}
                        >
                            <span>{teacher.teacher_name}</span>
                            {getTeacherSelectionStatus(teacher.teacher_id) && (
                                <div className="flex items-center gap-2">
                                    <span className="text-sm font-medium">
                                        Teacher {getTeacherSelectionStatus(teacher.teacher_id)}
                                    </span>
                                    <Check className="h-4 w-4" />
                                </div>
                            )}
                        </button>
                    ))}
                </div>
            </div>
        </motion.div>
    )
}

export default TeacherPanel
import React from 'react';
import { X, Check } from "lucide-react";
import { motion } from 'framer-motion';

const TeacherPanel = ({
    selectedTeacher,
    organizationTeachers,
    setIsTeacherPanelOpen,
    setSelectedTeacher
}) => {
    const handleTeacherSelect = (teacherId) => {
        setSelectedTeacher(teacherId);
        setIsTeacherPanelOpen(false);
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
                        Select Teacher
                    </h3>
                    <button
                        onClick={() => setIsTeacherPanelOpen(false)}
                        className="text-white/70 hover:text-white transition-colors"
                    >
                        <X className="h-5 w-5" />
                    </button>
                </div>
                <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-2">
                    {organizationTeachers.map(teacher => (
                        <button
                            key={teacher.userId}
                            onClick={() => handleTeacherSelect(teacher.userId)}
                            className={`w-full flex items-center justify-between bg-zinc-800 text-white rounded-lg px-4 py-3 hover:bg-zinc-700 transition-colors ${selectedTeacher === teacher.userId ? 'bg-white text-black hover:bg-white' : ''
                                }`}
                        >
                            <span>{teacher.name}</span>
                            {selectedTeacher === teacher.userId && (
                                <Check className="h-4 w-4" />
                            )}
                        </button>
                    ))}
                </div>
            </div>
        </motion.div>
    );
};

export default TeacherPanel;
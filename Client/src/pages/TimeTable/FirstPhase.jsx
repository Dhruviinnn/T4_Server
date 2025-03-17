import React from 'react'
import { Clock, PlusCircle } from 'lucide-react'
import { motion } from 'framer-motion'
import { toast } from 'sonner';

const FirstPhase = ({ step, setStep, classname, setClassname, division, setDivision, startTime, setStartTime, hoursPerDay, setHoursPerDay, periodDuration, setPeriodDuration, specialHours, setSpecialHours, breakDuration, setBreakDuration }) => {
    const hoursArray = [4, 5, 6, 7, 8, 9];
    const durationArray = [30, 45, 60, 90];
    const specialDurationArray = [1, 2, 3];
    const breakDurationsArray = [30, 45, 60];
    const handleNextStep = () => {
        if (!classname || !division || !startTime) {
            toast.error('Please fill in all required fields', {
                position: 'bottom-right',
                className: 'bg-red-500'
            });
            return;
        }
        setStep(2);
    };
    return (

        <motion.div
            className="w-full"
            initial={{ x: 0 }}
            animate={{ x: step === 2 ? -800 : 0, opacity: step === 2 ? 0 : 1 }}
            transition={{ duration: 0.5 }}
        >
            <h2 className="text-white text-xl font-semibold mb-6">Create Timetable</h2>
            <form className="space-y-6">
                <div className="flex flex-col sm:flex-row gap-4 sm:gap-x-8">
                    <div className="group flex-1">
                        <label htmlFor="classname" className="block text-md font-medium text-white/70 mb-2 group-focus-within:text-white transition-colors duration-200">
                            Class Name
                        </label>
                        <input
                            type="text"
                            id="classname"
                            value={classname}
                            onChange={(e) => setClassname(e.target.value)}
                            className="w-full px-4 py-2 bg-zinc-800 border border-white/10 rounded-lg text-white placeholder-white/30 focus:outline-none focus:ring-2 focus:ring-white/50 focus:border-transparent transition-all duration-200"
                            required
                        />
                    </div>
                    <div className="group flex-1">
                        <label htmlFor="division" className="block text-md font-medium text-white/70 mb-2 group-focus-within:text-white transition-colors duration-200">
                            Division
                        </label>
                        <input
                            type="text"
                            id="division"
                            value={division}
                            onChange={(e) => setDivision(e.target.value)}
                            className="w-full px-4 py-2 bg-zinc-800 border border-white/10 rounded-lg text-white placeholder-white/30 focus:outline-none focus:ring-2 focus:ring-white/50 focus:border-transparent transition-all duration-200"
                            required
                        />
                    </div>
                </div>
                <div className="flex flex-col sm:flex-row gap-4 sm:gap-x-8">
                    {/* Time Picker */}
                    {/* Hours Per Day */}
                    <div className="group flex-1">
                        <label htmlFor="startTime" className="block text-md font-medium text-white/70 mb-2 group-focus-within:text-white transition-colors duration-200">
                            Start Time
                        </label>
                        <div className="relative">
                            <input
                                type="time"
                                id="startTime"
                                value={startTime}
                                onChange={(e) => setStartTime(e.target.value)}
                                className="w-full px-4 py-2 bg-zinc-800 border border-white/10 rounded-lg text-white placeholder-white/30 focus:outline-none focus:ring-2 focus:ring-white/50 focus:border-transparent transition-all duration-200 appearance-none"
                                required
                            />
                            <Clock className="absolute right-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-white/50" />
                        </div>
                    </div>

                    <div className="flex-1">
                        <label className="block text-md font-medium text-white/70 mb-2">Hours Per Day</label>
                        <div className="relative p-1.5 bg-zinc-800 rounded-full flex shadow-inner w-full">
                            <div
                                className="absolute top-0 left-0 h-[calc(100%)] w-[calc(16.66%)] bg-white rounded-full transition-all duration-300"
                                style={{
                                    left: `calc(${(hoursArray.indexOf(hoursPerDay) / hoursArray.length) * 100}%)`,
                                }}
                            ></div>
                            {hoursArray.map(hours => (
                                <button
                                    type="button"
                                    key={hours}
                                    onClick={() => setHoursPerDay(hours)}
                                    className={`flex-1 py-2 px-2 sm:px-4 rounded-full text-sm sm:text-md font-medium relative z-10 transition-colors duration-300 cursor-pointer ${hoursPerDay === hours ? 'text-black' : 'text-white/70 hover:text-white'}`}
                                >
                                    {hours}h
                                </button>
                            ))}
                        </div>
                    </div>
                </div>
                {/* Period Duration */}
                <div>
                    <label className="block text-md font-medium text-white/70 mb-2">Period Duration</label>
                    <div className="relative p-1.5 bg-zinc-800 rounded-full flex flex-wrap sm:flex-nowrap shadow-inner w-full">
                        <div
                            className="absolute top-0 left-0 h-[calc(100%)] w-[calc(25%)] bg-white rounded-full transition-all duration-300"
                            style={{
                                left: `calc(${(durationArray.indexOf(periodDuration) / durationArray.length) * 100}%)`,
                            }}
                        ></div>
                        {durationArray.map(item => (
                            <button
                                type="button"
                                key={item}
                                onClick={() => setPeriodDuration(item)}
                                className={`flex-1 py-2 px-2 sm:px-4 rounded-full text-sm sm:text-md font-medium relative z-10 transition-colors duration-300 cursor-pointer ${periodDuration === item ? 'text-black' : 'text-white/70 hover:text-white'}`}
                            >
                                {item} mins
                            </button>
                        ))}
                    </div>
                </div>

                {/* Special Subject/Lab Hours */}
                <div>
                    <label className="block text-md font-medium text-white/70 mb-2">Special Subject / Lab Hours</label>
                    <div className="relative p-1.5 bg-zinc-800 rounded-full flex shadow-inner w-full">
                        <div
                            className="absolute top-0 left-0 h-[calc(100%)] w-[calc(33.33%)] bg-white rounded-full transition-all duration-300"
                            style={{
                                left: `calc(${(specialDurationArray.indexOf(specialHours) / specialDurationArray.length) * 100}%)`,
                            }}
                        ></div>
                        {specialDurationArray.map(item => (
                            <button
                                type="button"
                                key={item}
                                onClick={() => setSpecialHours(item)}
                                className={`flex-1 py-2 px-2 sm:px-4 rounded-full text-sm sm:text-md font-medium relative z-10 transition-colors duration-300 cursor-pointer ${specialHours === item ? 'text-black' : 'text-white/70 hover:text-white'}`}
                            >
                                {item} hours
                            </button>
                        ))}
                    </div>
                </div>

                {/* Break Duration */}
                <div>
                    <label className="block text-md font-medium text-white/70 mb-2">Break Duration</label>
                    <div className="relative p-1.5 bg-zinc-800 rounded-full flex shadow-inner w-full">
                        <div
                            className="absolute top-0 left-0 h-[calc(100%)] w-[calc(33.33%)] bg-white rounded-full transition-all duration-300"
                            style={{
                                left: `calc(${(breakDurationsArray.indexOf(breakDuration) / breakDurationsArray.length) * 100}%)`,
                            }}
                        ></div>
                        {breakDurationsArray.map(item => (
                            <button
                                type="button"
                                key={item}
                                onClick={() => setBreakDuration(item)}
                                className={`flex-1 py-2 px-2 sm:px-4 rounded-full text-sm sm:text-md font-medium relative z-10 transition-colors duration-300 cursor-pointer ${breakDuration === item ? 'text-black' : 'text-white/70 hover:text-white'}`}
                            >
                                {item} mins
                            </button>
                        ))}
                    </div>
                </div>

                <button
                    type="button"
                    onClick={handleNextStep}
                    className="w-full bg-white text-black font-medium py-3 px-4 rounded-lg transition-all duration-300 hover:bg-zinc-100 active:bg-zinc-200 hover:scale-[1.02] flex items-center justify-center gap-3 mt-6"
                >
                    <PlusCircle className="h-5 w-5" />
                    Continue to Add Subjects
                </button>
            </form>
        </motion.div>
    )
}

export default FirstPhase
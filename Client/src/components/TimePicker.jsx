import React, { useState, useEffect } from 'react';
import { Clock } from 'lucide-react';

const TimePicker = ({ value, onChange }) => {
    const [isOpen, setIsOpen] = useState(false);
    const [selectedHour, setSelectedHour] = useState(12);
    const [selectedMinute, setSelectedMinute] = useState(0);
    const [period, setPeriod] = useState('AM');

    useEffect(() => {
        if (value) {
            const [hours, minutes] = value.split(':');
            let hour = parseInt(hours);
            if (hour >= 12) {
                setPeriod('PM');
                if (hour > 12) hour -= 12;
            } else {
                setPeriod('AM');
                if (hour === 0) hour = 12;
            }
            setSelectedHour(hour);
            setSelectedMinute(parseInt(minutes));
        }
    }, [value]);

    const handleTimeSelect = () => {
        let hours = selectedHour;
        if (period === 'PM' && hours !== 12) hours += 12;
        if (period === 'AM' && hours === 12) hours = 0;
        
        const timeString = `${hours.toString().padStart(2, '0')}:${selectedMinute.toString().padStart(2, '0')}`;
        onChange(timeString);
        setIsOpen(false);
    };

    const hours = Array.from({ length: 12 }, (_, i) => i + 1);
    const minutes = Array.from({ length: 60 }, (_, i) => i);

    return (
        <div>
            <div
                onClick={() => setIsOpen(!isOpen)}
                className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white cursor-pointer flex items-center justify-between hover:bg-white/10 hover:border-white/20 transition-all"
            >
                <span>
                    {value ? 
                        `${selectedHour.toString().padStart(2, '0')}:${selectedMinute.toString().padStart(2, '0')} ${period}` 
                        : 'Select Time'}
                </span>
                <Clock className="h-4 w-4" />
            </div>

            {isOpen && (
                <div className="absolute top-0 left-0 translate-x-[50%] z-50 mt-2 p-4 bg-[#1a1a1a] rounded-xl border border-white/10 shadow-xl w-[280px]">
                    <div className="text-center mb-6">
                        <h3 className="text-white/70 text-sm font-medium mb-2">SELECT TIME</h3>
                        <div className="text-white text-4xl font-light">
                            {selectedHour.toString().padStart(2, '0')}:{selectedMinute.toString().padStart(2, '0')}
                            <span className="text-2xl ml-2">{period}</span>
                        </div>
                    </div>

                    <div className="relative w-48 h-48 mx-auto mb-4">
                        <div className="absolute inset-0 rounded-full border-2 border-white/10">
                            {hours.map((hour) => {
                                const angle = ((hour - 3) * 30 * Math.PI) / 180;
                                const x = Math.cos(angle) * 80 + 96;
                                const y = Math.sin(angle) * 80 + 96;
                                return (
                                    <button
                                        key={hour}
                                        onClick={() => setSelectedHour(hour)}
                                        className={`absolute w-8 h-8 -mt-4 -ml-4 rounded-full flex items-center justify-center transition-colors
                                            ${selectedHour === hour ? 'bg-blue-400 text-white' : 'text-white/70 hover:bg-white/10'}`}
                                        style={{ left: `${x}px`, top: `${y}px` }}
                                    >
                                        {hour}
                                    </button>
                                );
                            })}
                            <div 
                                className="absolute left-1/2 top-1/2 w-1 h-[80px] -ml-[0.5px] -mt-[80px] origin-bottom transform transition-transform bg-blue-400"
                                style={{ 
                                    transform: `rotate(${((selectedHour) * 30)}deg)`
                                }}
                            />
                        </div>
                    </div>

                    <div className="flex justify-between items-center mt-4">
                        <button
                            onClick={() => setPeriod(period === 'AM' ? 'PM' : 'AM')}
                            className="px-3 py-1 rounded-lg text-white/70 hover:bg-white/10 transition-colors"
                        >
                            {period}
                        </button>
                        <div className="flex gap-2">
                            <button
                                onClick={() => setIsOpen(false)}
                                className="px-4 py-2 rounded-lg text-white/70 hover:bg-white/10 transition-colors"
                            >
                                CANCEL
                            </button>
                            <button
                                onClick={handleTimeSelect}
                                className="px-4 py-2 rounded-lg bg-blue-400 text-white hover:bg-blue-500 transition-colors"
                            >
                                OK
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default TimePicker;
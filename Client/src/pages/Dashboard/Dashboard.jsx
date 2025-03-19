import React, { useState, useMemo, useEffect } from 'react';
import { School, Users, Building2, Share2 } from 'lucide-react';
import Navbar from '../../components/Navbar';
import OrganizationView from './OrganizationView';
import ConfirmationDialog from './ConfirmationDialog';
import ScheduleStudentView from './ScheduleStudentView';
import ScheduleTeacherView from './ScheduleTeacherView';
import WeeklyTimetableModal from './WeeklyTimetableModel';
import { Helmet } from "react-helmet-async";
import { toast } from 'sonner'
import ToastProvider from '../../components/Toaster'
import { encode, decode } from 'js-base64'
import { useNavigate } from 'react-router-dom'
import { useUser } from '../../contexts/user.context';
import { userFetcher } from '../../lib/userFetcher';

const mockTimetables = [
    {
        id: 1,
        className: "Class X",
        division: "A",
        createdAt: "2024-01-15",
        totalSubjects: 6,
        totalTeachers: 8
    },
    {
        id: 2,
        className: "Class XI",
        division: "B",
        createdAt: "2024-01-16",
        totalSubjects: 7,
        totalTeachers: 9
    }
];
const mockTeacherSchedule = {
    Monday: [
        { time: "9:00 AM", subject: "Mathematics", class: "X-A", duration: "45 mins" },
        { time: "10:00 AM", subject: "Mathematics", class: "XI-B", duration: "45 mins" },
        { time: "11:00 AM", subject: "Mathematics", class: "IX-C", duration: "45 mins" }
    ],
    Tuesday: [
        { time: "9:00 AM", subject: "Mathematics", class: "XII-A", duration: "45 mins" },
        { time: "10:00 AM", subject: "Mathematics", class: "X-B", duration: "45 mins" },
        { time: "2:00 PM", subject: "Mathematics", class: "XI-A", duration: "45 mins" }
    ],
    Wednesday: [
        { time: "10:00 AM", subject: "Mathematics", class: "IX-A", duration: "45 mins" },
        { time: "11:00 AM", subject: "Mathematics", class: "X-A", duration: "45 mins" },
        { time: "1:00 PM", subject: "Mathematics", class: "XII-B", duration: "45 mins" }
    ],
    Thursday: [
        { time: "9:00 AM", subject: "Mathematics", class: "XI-B", duration: "45 mins" },
        { time: "11:00 AM", subject: "Mathematics", class: "X-C", duration: "45 mins" },
        { time: "2:00 PM", subject: "Mathematics", class: "IX-B", duration: "45 mins" }
    ],
    Friday: [
        { time: "9:00 AM", subject: "Mathematics", class: "X-A", duration: "45 mins" },
        { time: "10:00 AM", subject: "Mathematics", class: "XII-A", duration: "45 mins" },
        { time: "1:00 PM", subject: "Mathematics", class: "XI-C", duration: "45 mins" }
    ],
    Saturday: [
        { time: "9:00 AM", subject: "Mathematics", class: "IX-A", duration: "45 mins" },
        { time: "11:00 AM", subject: "Mathematics", class: "X-B", duration: "45 mins" },
        { time: "12:00 PM", subject: "Mathematics", class: "XII-C", duration: "45 mins" }
    ]
};
const mockWeekSchedule = {
    Monday: [
        { time: "9:00 AM", subject: "Mathematics", teacher: "John Smith", duration: "45 mins" },
        { time: "10:00 AM", subject: "Physics", teacher: "Emma Johnson", duration: "45 mins" },
        { time: "11:00 AM", subject: "Chemistry", teacher: "David Lee", duration: "45 mins" }
    ],
    Tuesday: [
        { time: "9:00 AM", subject: "Biology", teacher: "Sophia Brown", duration: "45 mins" },
        { time: "10:00 AM", subject: "English", teacher: "Michael Davis", duration: "45 mins" },
        { time: "11:00 AM", subject: "History", teacher: "Olivia Martinez", duration: "45 mins" }
    ],
    Wednesday: [
        { time: "9:00 AM", subject: "Physics", teacher: "Emma Johnson", duration: "45 mins" },
        { time: "10:00 AM", subject: "Chemistry", teacher: "David Lee", duration: "45 mins" },
        { time: "11:00 AM", subject: "Mathematics", teacher: "John Smith", duration: "45 mins" }
    ],
    Thursday: [
        { time: "9:00 AM", subject: "English", teacher: "Michael Davis", duration: "45 mins" },
        { time: "10:00 AM", subject: "Biology", teacher: "Sophia Brown", duration: "45 mins" },
        { time: "11:00 AM", subject: "History", teacher: "Olivia Martinez", duration: "45 mins" }
    ],
    Friday: [
        { time: "9:00 AM", subject: "Mathematics", teacher: "John Smith", duration: "45 mins" },
        { time: "10:00 AM", subject: "Physics", teacher: "Emma Johnson", duration: "45 mins" },
        { time: "11:00 AM", subject: "English", teacher: "Michael Davis", duration: "45 mins" }
    ],
    Saturday: [
        { time: "9:00 AM", subject: "History", teacher: "Olivia Martinez", duration: "45 mins" },
        { time: "10:00 AM", subject: "Chemistry", teacher: "David Lee", duration: "45 mins" },
        { time: "11:00 AM", subject: "Biology", teacher: "Sophia Brown", duration: "45 mins" }
    ]
};

const ShareButton = ({ title, forX, user }) => (
    <button onClick={() => {
        const data = { orgId: user.userId, role: forX }
        const encodedURL = `http://localhost:5173/signup/${encode(JSON.stringify(data))}`
        navigator.clipboard.writeText(encodedURL)
        toast.success(`Link Copied 🎉`, {
            duration: 5000,
            style: { backgroundColor: "#16a34a", color: "white", fontSize: "1rem" },
        });
    }} className='flex items-center space-x-2 text-indigo-400 hover:text-indigo-300 px-3 py-2 rounded-lg hover:bg-indigo-400/10 transition-colors cursor-pointer' >
        <Share2 className="w-4 h-4" />
        <span>{title}</span>
    </button >
)

const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

const Dashboard = () => {
    const navigate = useNavigate()
    const [user, setUser] = useUser()
    const [selectedDay, setSelectedDay] = useState(() => {
        const today = new Date();
        return days[today.getDay() === 0 ? 6 : today.getDay() - 1];
    });
    const [selectedTimetable, setSelectedTimetable] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [absentClasses, setAbsentClasses] = useState(new Set());
    const [confirmDialog, setConfirmDialog] = useState({
        isOpen: false,
        scheduleKey: null,
        isUnmarking: false
    });
    const handleCloseModal = () => { setIsModalOpen(false) }

    useEffect(() => {
        userFetcher(user, setUser)
    }, []);

    const handleConfirmAbsent = () => { }
    const UserInfo = () => (
        <>
            <Helmet>
                <title>Dashboard | Time Fourthe</title>
                <link rel="icon" type="image/png" href="/home-icon.png" />
            </Helmet>

            <div className="animate-on-mount glass-effect rounded-xl p-6 transition-all duration-300 ">
                <ToastProvider />
                <div className="flex justify-between items-center gap-4">
                    <div className='flex items-center gap-x-1'>
                        <div className="p-4 glass-effect rounded-lg hover-scale">
                            {user.role === "organization" ? (
                                <Building2 className="h-6 w-6 text-white transition-all duration-300 hover:text-zinc-200" />
                            ) : user.role === "teacher" ? (
                                <Users className="h-6 w-6 text-white transition-all duration-300 hover:text-zinc-200" />
                            ) : (
                                <School className="h-6 w-6 text-white transition-all duration-300 hover:text-zinc-200" />
                            )}
                        </div>
                        <div className="slide-in">
                            <h2 className="text-white text-xl font-semibold tracking-tight">{user.name}</h2>
                            <p className="text-white/70 text-sm font-medium">ID: {user.userId}</p>
                        </div>
                    </div>
                    <div className='flex gap-x-2'>
                        <ShareButton url={'s'} title={'Share for Students'} forX={'student'} user={user} />
                        <ShareButton url={'s'} title={'Share for Teachers'} forX={'teacher'} user={user} />
                    </div>
                </div>
            </div>
        </>
    );

    const WeekNavigator = () => (
        <div className="animate-on-mount glass-effect rounded-xl p-6">
            <div className="flex flex-col w-max mx-auto sm:flex-row items-center justify-between gap-4">
                <div className="flex flex-wrap justify-center gap-2">
                    {days.map((day, index) => (
                        <button
                            key={day}
                            onClick={() => setSelectedDay(day)}
                            className={`px-4 py-2 rounded-lg text-sm font-medium transition-all duration-300 transform hover:-translate-y-1 ${selectedDay === day
                                ? 'bg-white text-black shadow-lg'
                                : 'glass-effect text-white hover:bg-white/15'
                                }`}
                            style={{
                                animation: `fadeIn 0.3s ease-out forwards ${index * 0.1}s`
                            }}
                        >
                            {window.innerWidth < 640 ? day.slice(0, 3) : day}
                        </button>
                    ))}
                </div>
            </div>
        </div>
    );

    return (
        <>
            <Navbar />
            <div className="min-h-screen bg-gradient-to-b from-black to-zinc-900 p-4 sm:p-6 md:p-8">
                <div className="max-w-7xl mx-auto space-y-6">
                    <UserInfo />
                    {user.role === 'teacher' && (
                        <div className="space-y-6">
                            <WeekNavigator />
                            <div className="animate-on-mount">
                                <ScheduleTeacherView
                                    // selectedDate={selectedDate}
                                    selectedDay={selectedDay}
                                    mockTeacherSchedule={mockTeacherSchedule}
                                    // currentDate={currentDate}
                                    days={days}
                                    absentClasses={absentClasses}
                                    setConfirmDialog={setConfirmDialog}
                                />
                            </div>
                        </div>
                    )}
                    {user.role === 'student' && (
                        <div className="space-y-6">
                            <WeekNavigator />
                            <div className="animate-on-mount">
                                <ScheduleStudentView
                                    mockWeekSchedule={mockWeekSchedule}
                                    selectedDay={selectedDay}
                                />
                            </div>
                        </div>
                    )}
                    {user.role === "organization" && (
                        <div className="animate-on-mount">
                            <OrganizationView
                                mockTimetables={mockTimetables}
                                setIsModalOpen={setIsModalOpen}
                                setSelectedTimetable={setSelectedTimetable}
                            />
                        </div>
                    )}
                    {selectedTimetable && (
                        <WeeklyTimetableModal
                            timetable={selectedTimetable}
                            onClose={handleCloseModal}
                            isOpen={isModalOpen}
                            days={days}
                            mockWeekSchedule={mockWeekSchedule}
                        />
                    )}
                    <ConfirmationDialog
                        isOpen={confirmDialog.isOpen}
                        onClose={() => setConfirmDialog({ isOpen: false, scheduleKey: null, isUnmarking: false })}
                        onConfirm={handleConfirmAbsent}
                        message={
                            confirmDialog.isUnmarking
                                ? "Are you sure you want to unmark this class as absent?"
                                : "Are you sure you want to mark this class as absent?"
                        }
                    />
                </div>
            </div>
        </>
    );
};

export default Dashboard;
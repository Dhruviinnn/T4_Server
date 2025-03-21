import React, { useState, useMemo, useEffect } from 'react';
import { School, Users, Building2, Share2 } from 'lucide-react';
import { Helmet } from "react-helmet-async";
import { toast } from 'sonner';
import { encode, decode } from 'js-base64';
import { useNavigate } from 'react-router-dom';

import Navbar from '../../components/Navbar';
import OrganizationView from './OrganizationView';
import ConfirmationDialog from './ConfirmationDialog';
import ScheduleStudentView from './ScheduleStudentView';
import ScheduleTeacherView from './ScheduleTeacherView';
import WeeklyTimetableModal from './WeeklyTimetableModel'
import ToastProvider from '../../components/Toaster';
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
  "Monday": [
    { "time": 540, "subject": "Mathematics", "class": "Class-10", "duration": 45 },
    { "time": 600, "subject": "Mathematics", "class": "Class-11B", "duration": 45 },
    { "time": 660, "subject": "Mathematics", "class": "Class-9C", "duration": 45 },
    { "time": 600, "subject": "Physics", "class": "Class-Physics", "duration": 45 }
  ],
  "Tuesday": [
    { "time": 540, "subject": "Mathematics", "class": "Class-10", "duration": 45 },
    { "time": 600, "subject": "Mathematics", "class": "Class-10B", "duration": 45 },
    { "time": 840, "subject": "Mathematics", "class": "Class-11A", "duration": 45 },
    { "time": 660, "subject": "Physics", "class": "Class-Physics", "duration": 45 }
  ],
  "Wednesday": [
    { "time": 600, "subject": "Mathematics", "class": "Class-10", "duration": 45 },
    { "time": 660, "subject": "Mathematics", "class": "Class-10A", "duration": 45 },
    { "time": 780, "subject": "Mathematics", "class": "Class-12B", "duration": 45 },
    { "time": 600, "subject": "Physics", "class": "Class-Physics", "duration": 45 }
  ],
  "Thursday": [
    { "time": 540, "subject": "Mathematics", "class": "Class-10", "duration": 45 },
    { "time": 660, "subject": "Mathematics", "class": "Class-10C", "duration": 45 },
    { "time": 840, "subject": "Mathematics", "class": "Class-9B", "duration": 45 },
    { "time": 780, "subject": "Physics", "class": "Class-Physics", "duration": 45 }
  ],
  "Friday": [
    { "time": 540, "subject": "Mathematics", "class": "Class-10", "duration": 45 },
    { "time": 600, "subject": "Mathematics", "class": "Class-12A", "duration": 45 },
    { "time": 780, "subject": "Mathematics", "class": "Class-11C", "duration": 45 },
    { "time": 600, "subject": "Physics", "class": "Class-Physics", "duration": 45 }
  ],
  "Saturday": [
    { "time": 540, "subject": "Mathematics", "class": "Class-10", "duration": 45 },
    { "time": 660, "subject": "Mathematics", "class": "Class-10B", "duration": 45 },
    { "time": 720, "subject": "Mathematics", "class": "Class-12C", "duration": 45 },
    { "time": 780, "subject": "Physics", "class": "Class-Physics", "duration": 45 }
  ]
}

const mockWeekSchedule = {
  "Monday": [
    { "startTime": 600, "subject": "Physics", "teacher": "Frank_Physics" },
    { "startTime": 660, "subject": "Chemistry", "teacher": "Henry_Chem" },
    { "startTime": 720, "subject": "History", "teacher": "Eva_History" },
    { "startTime": 780, "subject": "Science", "teacher": "Charlie_Sci" }
  ],
  "Tuesday": [
    { "startTime": 600, "subject": "History", "teacher": "Eva_History" },
    { "startTime": 660, "subject": "Physics", "teacher": "Frank_Physics" },
    { "startTime": 720, "subject": "Biology", "teacher": "Grace_Biology" },
    { "startTime": 780, "subject": "Science", "teacher": "Charlie_Sci" }
  ],
  "Wednesday": [
    { "startTime": 600, "subject": "Physics", "teacher": "Frank_Physics" },
    { "startTime": 660, "subject": "Science", "teacher": "Charlie_Sci" },
    { "startTime": 720, "subject": "Biology", "teacher": "Grace_Biology" },
    { "startTime": 780, "subject": "History", "teacher": "Eva_History" }
  ],
  "Thursday": [
    { "startTime": 600, "subject": "History", "teacher": "Eva_History" },
    { "startTime": 660, "subject": "Chemistry", "teacher": "Henry_Chem" },
    { "startTime": 720, "subject": "Science", "teacher": "Charlie_Sci" },
    { "startTime": 780, "subject": "Physics", "teacher": "Frank_Physics" }
  ]
};

const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

const convertToSimpleTime = (x) => {
  let hours = Math.floor(x / 60);
  let mins = x % 60;
  let period = hours >= 12 ? "PM" : "AM";

  hours = hours % 12 || 12; // Convert 0 to 12-hour format
  console.log(x % 60);

  return `${hours}:${mins.toString().padStart(2, '0')} ${period}`;
}

const ShareButton = ({ title, forX, user }) => (
  <button
    onClick={() => {
      const data = { orgId: user.userId, role: forX };
      const encodedURL = `http://localhost:5173/signup/${encode(JSON.stringify(data))}`;
      navigator.clipboard.writeText(encodedURL);
      toast.success(`Link Copied 🎉`, {
        duration: 5000,
        style: { backgroundColor: "#16a34a", color: "white", fontSize: "1rem" },
      });
    }}
    className="flex items-center space-x-2 text-indigo-400 hover:text-indigo-300 px-3 py-2 rounded-lg hover:bg-indigo-400/10 transition-colors cursor-pointer"
  >
    <Share2 className="w-4 h-4" />
    <span>{title}</span>
  </button>
);

const WeekNavigator = ({
  selectedDay,
  setSelectedDay,
}) => (
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
              animation: `fadeIn 0.3s ease-out forwards ${index * 0.1}s`,
            }}
          >
            {window.innerWidth < 640 ? day.slice(0, 3) : day}
          </button>
        ))}
      </div>
    </div>
  </div>
);

const Dashboard = () => {
  const navigate = useNavigate();
  const [user, setUser] = useUser();
  const [selectedDay, setSelectedDay] = useState(() => {
    const today = new Date();
    return days[today.getDay() === 0 ? 6 : today.getDay() - 1];
  });
  const [selectedTimetable, setSelectedTimetable] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [absentClasses, setAbsentClasses] = useState([]);
  const [confirmDialog, setConfirmDialog] = useState({
    isOpen: false,
    scheduleKey: null,
    isUnmarking: false,
    subject: null,
    className: null,
    date: null
  });

  useEffect(() => {
    userFetcher(user, setUser);
    const isAbsentClassesExist = localStorage.getItem('absentClasses')
    if (isAbsentClassesExist) {
      const storedAbsentClasses = JSON.parse(decode(isAbsentClassesExist))
      if (storedAbsentClasses) setAbsentClasses(storedAbsentClasses)
    }
  }, []);

  const handleCloseModal = () => setIsModalOpen(false);
  const handleConfirmAbsent = (e) => {
    setConfirmDialog({
      isOpen: false,
      scheduleKey: null,
      isUnmarking: false,
      subject: null,
      className: null,
      date: confirmDialog.date,
      deleteTableId: null
    })
    if (user.role == 'organization') {
      console.log("TimeTable Delete : ", confirmDialog.deleteTableId);
    }
    else {
      setAbsentClasses((prev) => [...prev, confirmDialog.scheduleKey])
    }
  };
  useEffect(() => {
    localStorage.setItem('absentClasses', encode(JSON.stringify(absentClasses)))
  }, [absentClasses])

  const UserInfo = () => (
    <>
      <Helmet>
        <title>Dashboard | Time Fourthe</title>
        <link rel="icon" type="image/png" href="/home-icon.png" />
      </Helmet>

      <div className="animate-on-mount glass-effect rounded-xl p-6 transition-all duration-300">
        <ToastProvider />
        <div className="flex justify-between items-center gap-4">
          <div className="flex items-center gap-x-1">
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
          {
            user.role == 'organization' &&
            <div className="flex gap-x-2">
              <ShareButton title="Share for Students" forX="student" user={user} />
              <ShareButton title="Share for Teachers" forX="teacher" user={user} />
            </div>
          }
        </div>
      </div>
    </>
  );

  return (
    <>
      <Navbar role={user.role} />
      <div className="min-h-screen bg-gradient-to-b from-black to-zinc-900 p-4 sm:p-6 md:p-8">
        <div className="max-w-7xl mx-auto space-y-6">
          <UserInfo />
          {user.role === 'teacher' && (
            <div className="space-y-6">
              <WeekNavigator selectedDay={selectedDay} setSelectedDay={setSelectedDay} />
              <div className="animate-on-mount">
                <ScheduleTeacherView
                  selectedDay={selectedDay}
                  convertToSimpleTime={convertToSimpleTime}
                  mockTeacherSchedule={mockTeacherSchedule}
                  days={days}
                  absentClasses={absentClasses}
                  setAbsentClasses={setAbsentClasses}
                  setConfirmDialog={setConfirmDialog}
                />
              </div>
            </div>
          )}
          {user.role === 'student' && (
            <div className="space-y-6">
              <WeekNavigator selectedDay={selectedDay} setSelectedDay={setSelectedDay} />
              <div className="animate-on-mount">
                <ScheduleStudentView
                  convertToSimpleTime={convertToSimpleTime}
                  mockWeekSchedule={mockWeekSchedule}
                  selectedDay={selectedDay}
                />
              </div>
            </div>
          )}
          {user.role === "organization" && (
            <div className="animate-on-mount">
              <OrganizationView
                confirmDialog={confirmDialog}
                mockTimetables={mockTimetables}
                setConfirmDialog={setConfirmDialog}
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
              user.role == 'organization' ? 'Are you sure you want to delete this timetable?' :
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
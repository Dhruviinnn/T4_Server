import { useState, useEffect } from "react";
import { Toaster, toast } from 'sonner';
import { AnimatePresence } from "framer-motion";
import Navbar from "../../components/Navbar";
import FirstPhase from "./FirstPhase";
import SecondPhase from "./SecondPhase";
import TeacherPanel from "./TeacherPanel";
import { Helmet } from "react-helmet-async";
import { userFetcher } from '../../lib/userFetcher';
import { useUser } from "../../contexts/user.context";
import { decode, encode } from "js-base64";

const TimeTableForm = () => {
	const [user, setUser] = useUser()
	const [step, setStep] = useState(1);
	const [organizationTeachers, setOrganizationTeachers] = useState({});
	const [periodDuration, setPeriodDuration] = useState(30);
	const [specialHours, setSpecialHours] = useState(1);
	const [hoursPerDay, setHoursPerDay] = useState(6);
	const [breakDuration, setBreakDuration] = useState(30);
	const [classname, setClassname] = useState("");
	const [division, setDivision] = useState("");
	const [subjects, setSubjects] = useState([]);
	const [startTime, setStartTime] = useState("")
	const [newSubject, setNewSubject] = useState("");
	const [selectedTeacher, setSelectedTeacher] = useState("");
	const [isTeacherPanelOpen, setIsTeacherPanelOpen] = useState(false);
	const [selectingSecondTeacher, setSelectingSecondTeacher] = useState(false);

	useEffect(() => {
		userFetcher(user, setUser)

		const x = JSON.parse(decode('W3sidXNlcklkIjoiNjdkOTZmOGUyMjM0NTg0ZDg3NzgzMDhiIiwibmFtZSI6IkhhYmliaV8xMiJ9LHsidXNlcklkIjoiNjdkOTZmOGUyMjM0NTg0ZDg3NzgzMDhjIiwibmFtZSI6IkFsaWNlX1dvbmRlciJ9LHsidXNlcklkIjoiNjdkOTZmOGUyMjM0NTg0ZDg3NzgzMDhkIiwibmFtZSI6IkJvYl9NYXRoIn0seyJ1c2VySWQiOiI2N2Q5NmY4ZTIyMzQ1ODRkODc3ODMwOGUiLCJuYW1lIjoiQ2hhcmxpZV9TY2kifSx7InVzZXJJZCI6IjY3ZDk2ZjhlMjIzNDU4NGQ4Nzc4MzA4ZiIsIm5hbWUiOiJEYXZpZF9FbmcifSx7InVzZXJJZCI6IjY3ZDk2ZjhlMjIzNDU4NGQ4Nzc4MzA5MCIsIm5hbWUiOiJFdmFfSGlzdG9yeSJ9LHsidXNlcklkIjoiNjdkOTZmOGUyMjM0NTg0ZDg3NzgzMDkxIiwibmFtZSI6IkZyYW5rX1BoeXNpY3MifSx7InVzZXJJZCI6IjY3ZDk2ZjhlMjIzNDU4NGQ4Nzc4MzA5MiIsIm5hbWUiOiJHcmFjZV9CaW9sb2d5In0seyJ1c2VySWQiOiI2N2Q5NmY4ZTIyMzQ1ODRkODc3ODMwOTMiLCJuYW1lIjoiSGVucnlfQ2hlbSJ9LHsidXNlcklkIjoiNjdkOTZmOGUyMjM0NTg0ZDg3NzgzMDk0IiwibmFtZSI6Ikl2eV9HZW8ifSx7InVzZXJJZCI6IjY3ZDk2ZjhlMjIzNDU4NGQ4Nzc4MzA5NSIsIm5hbWUiOiJKYWNrX1BzeWNoIn0seyJ1c2VySWQiOiI2N2Q5NmY4ZTIyMzQ1ODRkODc3ODMwOTYiLCJuYW1lIjoiS2FyZW5fQXJ0cyJ9LHsidXNlcklkIjoiNjdkOTZmOGUyMjM0NTg0ZDg3NzgzMDk3IiwibmFtZSI6Ikxlb19QRSJ9LHsidXNlcklkIjoiNjdkOTZmOGUyMjM0NTg0ZDg3NzgzMDk4IiwibmFtZSI6Ik1pYV9NdXNpYyJ9LHsidXNlcklkIjoiNjdkOTZmOGUyMjM0NTg0ZDg3NzgzMDk5IiwibmFtZSI6Ik5vYWhfQ29tcFNjaSJ9XQ'))

		console.log('teacherList : ', x);

	}, [])

	useEffect(() => {
		// Teachers fetching
		if (user.userId) {
			const teachers = localStorage.getItem('teachers');
			if (!teachers) {
				const OrgId = user.userId;
				console.log(OrgId);
				fetch(`http://localhost:3000/api/get/teachers?OrgId=${OrgId}`)
					.then(res => res.json())
					.then(data => {
						if (data) {
							setOrganizationTeachers(data);
							localStorage.setItem('teachers', encode(JSON.stringify(data)))
						}
					})
			}
			else {
				setOrganizationTeachers(JSON.parse(decode(teachers)))
			}
		}
	}, [user])

	const handleSubmit = (e) => {
		e.preventDefault();
		if (subjects.length === 0) {
			toast.error('Please add at least one subject', {
				position: 'bottom-right',
				className: 'bg-red-500'
			});
			return;
		}
		console.log({
			classname,
			division,
			startTime,
			hoursPerDay,
			periodDuration,
			specialHours,
			breakDuration,
			subjects
		});
	};


	return (
		<>
			<Helmet>
				<title>TimeTable | Time Fourthe</title>
				<link rel="icon" type="image/png" href="/home-icon.png" />
			</Helmet>
			<Toaster
				position="bottom-right"
				toastOptions={{
					style: {
						fontSize: '1rem',
						background: 'rgb(239 68 68)',
						color: 'white',
						border: 'none'
					},
					className: 'bg-red-500'
				}}
			/>
			<Navbar />
			<div className="min-h-screen bg-black flex items-center justify-center py-4 px-4 sm:px-20 relative">
				<div className="w-full bg-zinc-900 rounded-xl shadow-2xl shadow-white/5 p-4 sm:p-6 md:p-8 border border-white/10">
					<div className="relative">
						{/* Step 1: Basic Form */}
						<FirstPhase
							step={step} setStep={setStep}
							classname={classname} setClassname={setClassname}
							division={division} setDivision={setDivision}
							startTime={startTime} setStartTime={setStartTime}
							hoursPerDay={hoursPerDay} setHoursPerDay={setHoursPerDay}
							periodDuration={periodDuration} setPeriodDuration={setPeriodDuration}
							specialHours={specialHours} setSpecialHours={setSpecialHours}
							breakDuration={breakDuration} setBreakDuration={setBreakDuration}
						/>

						{/* Step 2: Subject Creation and Display */}
						<AnimatePresence>
							{step === 2 && (
								<SecondPhase
									newSubject={newSubject} setNewSubject={setNewSubject}
									selectedTeacher={selectedTeacher} setSelectedTeacher={setSelectedTeacher}
									subjects={subjects} setSubjects={setSubjects}
									setSelectingSecondTeacher={setSelectingSecondTeacher}
									setIsTeacherPanelOpen={setIsTeacherPanelOpen}
									organizationTeachers={organizationTeachers}
									handleSubmit={handleSubmit}
								/>
							)}
						</AnimatePresence>
					</div>
				</div>

				{/* Teacher Selection Panel */}
				<AnimatePresence>
					{isTeacherPanelOpen && (
						<TeacherPanel
							selectingSecondTeacher={selectingSecondTeacher}
							organizationTeachers={organizationTeachers}
							setIsTeacherPanelOpen={setIsTeacherPanelOpen}
							selectedTeacher={selectedTeacher}
							setSelectedTeacher={setSelectedTeacher}
							setSelectingSecondTeacher={setSelectingSecondTeacher}
						/>
					)}
				</AnimatePresence>
			</div>
		</>
	);
}

export default TimeTableForm;
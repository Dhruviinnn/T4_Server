import React from 'react'
import { School, GraduationCap, Building2 } from 'lucide-react'
const userTypes = [
    {
        id: 'teacher',
        title: 'Teacher',
        icon: School,
        description: 'Create and manage courses, assignments, and student progress'
    },
    {
        id: 'student',
        title: 'Student',
        icon: GraduationCap,
        description: 'Access courses, submit assignments, and track your learning journey'
    },
    {
        id: 'organization',
        title: 'Organization',
        icon: Building2,
        description: 'Manage multiple courses and teachers under one institution'
    }
];
const ChooseRole = ({ setStep, selectedType, setSelectedType }) => {
    const handleRoleSubmit = () => {
        if (selectedType) {
            setStep('details');
        }
    };
    return (
        <>
            <h1 className="text-4xl md:text-5xl font-bold mb-6 bg-gradient-to-r from-gray-100 to-gray-300 text-transparent bg-clip-text mt-12">
                Choose your role
            </h1>
            <p className="text-gray-500 text-xl mb-12">
                Select the option that best describes you
            </p>

            <div className="space-y-4">
                {userTypes.map(({ id, title, icon: Icon, description }) => (
                    <button
                        key={id}
                        onClick={() => setSelectedType(id)}
                        className={`
                                            w-full p-6 rounded-xl border transition-all duration-300 backdrop-blur-sm text-left
                                            ${selectedType === id
                                ? 'border-white/20 bg-white/5'
                                : 'border-white/10 bg-white/[0.02] hover:bg-white/[0.04] hover:border-white/15 cursor-pointer'
                            }
                                        `}
                    >
                        <div className="flex items-start space-x-4">
                            <div className={`
                                                p-4 rounded-full transition-all duration-300 flex-shrink-0
                                                ${selectedType === id
                                    ? 'bg-white/10'
                                    : 'bg-white/5 group-hover:bg-white/8'
                                }
                                            `}>
                                <Icon
                                    size={28}
                                    className={`
                                                        transition-all duration-300
                                                        ${selectedType === id
                                            ? 'text-white'
                                            : 'text-gray-400 group-hover:text-gray-300'
                                        }
                                                    `}
                                />
                            </div>
                            <div>
                                <h3 className={`
                                                    text-2xl font-semibold mb-2 transition-all duration-300
                                                    ${selectedType === id
                                        ? 'text-white'
                                        : 'text-gray-300'
                                    }
                                                `}>
                                    {title}
                                </h3>
                                <p className="text-gray-500 text-lg">{description}</p>
                            </div>
                        </div>
                    </button>
                ))}

                <div className="flex justify-end mt-8">
                    <button
                        onClick={handleRoleSubmit}
                        disabled={!selectedType}
                        className={`
                                            px-12 py-3 rounded-xl font-semibold transition-all duration-300 text-xl cursor-pointer
                                            ${selectedType
                                ? 'bg-white text-black hover:bg-gray-200'
                                : 'bg-white/5 text-gray-500 cursor-not-allowed'
                            }
                                        `}
                    >
                        Continue
                    </button>
                </div>
            </div>
        </>
    )
}

export default ChooseRole
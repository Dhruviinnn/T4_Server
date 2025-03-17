import { useState, useRef } from 'react'
import { ArrowRight, Lock, Unlock } from 'lucide-react'

const OriginalSignUp = ({ setStep, selectedType }) => {
    const [email, setEmail] = useState("");
    const [name, setName] = useState("")
    const [orgId, setOrgId] = useState("")
    const [passwordType, setPasswordType] = useState("password")
    const [password, setPassword] = useState("");
    const passwordRef = useRef();

    const showPassword = (e) => {
        e.preventDefault();
        passwordRef.current.type = passwordRef.current.type === "password" ? "text" : "password"
        setPasswordType(passwordRef.current.type)
    };
    const handleDetailsSubmit = (e) => {
        e.preventDefault();
        const data = {
            email,
            name,
            password,
            role: selectedType,
            orgId
        };

        fetch('http://localhost:3000/api/user/signup', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(res => res.json())
            .then(data => console.log(data))
    };
    return (
        <>
            <div className="flex items-center space-x-4 mb-8">
                <button
                    onClick={() => setStep('role')}
                    className="text-gray-400 hover:text-white transition-colors flex items-center space-x-2"
                >
                    <ArrowRight className="rotate-180 cursor-pointer" size={20} />
                </button>
                <div>
                    <h1 className="text-white text-4xl font-bold">
                        Create account
                    </h1>
                    <p className="text-gray-400 mt-2">
                        as {selectedType && selectedType.charAt(0).toUpperCase() + selectedType.slice(1)}
                    </p>
                </div>
            </div>

            <form onSubmit={handleDetailsSubmit} className="space-y-4">
                <div>
                    <input
                        name='email'
                        type="text"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Email"
                        className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent transition-all"
                    />
                </div>
                <div>
                    <input
                        name='name'
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Name"
                        className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent transition-all"
                    />
                </div>
                {selectedType != 'organization' &&
                    <div>
                        <input
                            name='orgid'
                            type="text"
                            value={orgId}
                            onChange={(e) => setOrgId(e.target.value)}
                            placeholder="Organization Id"
                            className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent transition-all"
                        />
                    </div>
                }
                <div>
                    <div className="relative">
                        <input
                            name='password'
                            ref={passwordRef}
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            placeholder="Password"
                            className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent transition-all"
                        />
                        {
                            passwordType === "password" ?
                                <Lock className="absolute right-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-500 cursor-pointer hover:text-white transition-colors" onClick={showPassword} />
                                :
                                <Unlock className="absolute right-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-500 cursor-pointer hover:text-white transition-colors" onClick={showPassword} />
                        }
                    </div>
                </div>

                <button
                    type="submit"
                    className="w-full bg-white text-black py-3 rounded-xl hover:bg-gray-100 transition-colors flex items-center justify-center group cursor-pointer font-medium"
                >
                    Create account
                    <ArrowRight className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform" />
                </button>

                <div className="text-center mt-6">
                    <p className="text-gray-400">
                        Already have an account?{" "}
                        <a
                            href="/login"
                            className="text-white hover:text-gray-200 transition-colors cursor-pointer font-medium"
                        >
                            Log in
                        </a>
                    </p>
                </div>
            </form>
        </>
    )
}

export default OriginalSignUp